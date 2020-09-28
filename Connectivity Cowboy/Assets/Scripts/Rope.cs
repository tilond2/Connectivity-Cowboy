using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {
    [SerializeField]
    public LineRenderer lineRenderer;
    public List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float ropeSegLen = 0.25f;
    public int segmentLength = 35;
    public float lineWidth = 0.1f;
    public bool active;

    // Use this for initialization
    void Start() {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < segmentLength; i++) {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        active = true;
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetMouseButtonDown(0)){ active = false; }
        this.DrawRope();
        //this.ropeSegments[this.segmentLength - 3] = new Vector3 Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void FixedUpdate() {
        this.Simulate();
    }

    private void Simulate() {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -.25f);


        //RopeSegment lastSegment;
        for (int i = 1; i < this.segmentLength; i++) {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
            
        }

        this.ropeSegments[this.segmentLength - 1] = this.ropeSegments[this.segmentLength - 12];

        

        //CONSTRAINTS
        for (int i = 0; i < 50; i++) {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint() {
        //Constrant to Mouse
        RopeSegment firstSegment = this.ropeSegments[0];
        
        firstSegment.posNow = this.transform.position;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RopeSegment lastSegment = this.ropeSegments[this.segmentLength - 20];
        if (active) {
            lastSegment.posNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        this.ropeSegments[this.segmentLength - 20] = lastSegment;
        this.ropeSegments[0] = firstSegment;

        for (int i = 0; i < this.segmentLength - 1; i++) {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen) {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            } else if (dist < ropeSegLen) {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0) {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            } else {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope() {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++) {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos) {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}