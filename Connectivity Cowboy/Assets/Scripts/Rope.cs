using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {
    [SerializeField]
    public LineRenderer lineRenderer;
    public List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float ropeSegLen = 0.125f;
    public int segmentLength = 50;
    public float lineWidth = 0.1f;
    public bool active;
    public GameObject lassoCollision;
    public float radius = 10f;
    public float angle;
    public GameObject anchor;
    public GameObject roped;
    public bool caught = false;
    public float influence = 0f;

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
        //this.GetComponent<CircleCollider2D>(). = lassoCollision.transform.position;
        if(!caught && Input.GetMouseButtonDown(0)){ active = !active; }
        if (caught) active = true;
        this.DrawRope();
        //this.ropeSegments[this.segmentLength - 3] = new Vector3 Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }



    private void FixedUpdate() {
        if(caught){
            float fallSpeed = -.01f;
            /*if(this.roped.transform.position.x < -10.5|| this.roped.transform.position.x > 10.5) {
                this.roped.transform.Translate(new Vector3(0f, fallSpeed, 0f));
            }else*/
            if (this.roped.transform.position.x < this.transform.position.x) {
                if (influence > 0f) {
                    this.roped.transform.Translate(new Vector3(influence / 40f, fallSpeed, 0));
                } else if(this.roped.transform.position.x>-10.5){
                    this.roped.transform.Translate(new Vector3(-.05f, fallSpeed, 0));
                }
            } else {
                if(influence <= 0f) {
                    this.roped.transform.Translate(new Vector3(influence / 40f, fallSpeed, 0));
                } else if(this.roped.transform.position.x<10.5){
                    this.roped.transform.Translate(new Vector3(.05f, fallSpeed, 0));
                }
            }
            
        }
        this.Simulate();
        //lasso.offset = this.ropeSegments[this.segmentLength - 12].posNow;
    }

    private void Simulate() {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -.125f);


        //RopeSegment lastSegment;
        for (int i = 1; i < this.segmentLength; i++) {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
            
        }

        //lasso = this.ropeSegments[this.segmentLength - 1].posNow;

        this.ropeSegments[this.segmentLength - 1] = this.ropeSegments[this.segmentLength - 12];

        

        //CONSTRAINTS
        for (int i = 0; i < 50; i++) {
            this.ApplyConstraint();
        }

        this.lassoCollision.transform.position = this.ropeSegments[this.segmentLength - 12].posNow;
        //this.ropeSegments[this.segmentLength - 1].posNow = this.lassoCollision.transform.position;
        RopeSegment attachPoint = this.ropeSegments[this.segmentLength - 1];
        attachPoint.posNow = this.lassoCollision.transform.position;
        this.ropeSegments[this.segmentLength - 1] = attachPoint;
    }
    /*
    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
        Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
        float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
        return Vector2.Angle(vec1, vec2) * sign;
    }*/

    public static float AngleInRad(Vector2 vec1, Vector2 vec2) {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    //This returns the angle in degrees
    public static float AngleInDeg(Vector2 vec1, Vector2 vec2) {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }


    private void ApplyConstraint() {
        //Constrant to Mouse
        RopeSegment firstSegment = this.ropeSegments[0];

        firstSegment.posNow = this.transform.position;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
       
        RopeSegment lastSegment = this.ropeSegments[this.segmentLength/3];
        if (active) {
            Vector2 oldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 newMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);// = new Vector2();
                                                                                   //sqrt((x2-x1)^2+(y2-y1)^2)

            //on circle: x = rsin(theta)   y = rcos(theta)
            float t = (AngleInRad(anchor.transform.position, oldMouse));// AngleBetweenVector2(anchor.transform.position, oldMouse);
            angle = t;
            if (Mathf.Sqrt(((oldMouse.x - anchor.transform.position.x)*(oldMouse.x - anchor.transform.position.x)) +
                ((oldMouse.y - anchor.transform.position.y)*(oldMouse.y - anchor.transform.position.y))) > radius) {
               
                newMouse = new Vector2(anchor.transform.position.x + radius * Mathf.Cos(t),anchor.transform.position.y + radius * Mathf.Sin(t));
            }
            influence = anchor.transform.position.x + radius * Mathf.Cos(t);
            lastSegment.posNow = newMouse;
        }
        this.ropeSegments[this.segmentLength/3] = lastSegment;
        this.ropeSegments[0] = firstSegment;
        if(caught){
            RopeSegment end = this.ropeSegments[this.segmentLength - 1];
            end.posNow = new Vector2(this.roped.transform.position.x,this.roped.transform.position.y-.5f);
            this.ropeSegments[this.segmentLength - 1] = end;
        }

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