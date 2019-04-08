using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebCamFaceLandmarkDetector;
using OpenCvSharp;

public class FacePosture 
{
    public class Angles
    {
        // x-axis
        public double roll;
        // y-axis
        public double pitch;
        // z-axis
        public double yaw;

        public Angles()
        {
            roll = 0;
            pitch = 0;
            yaw = 0;
        }
    }

    // 当前的旋转角度
    public Angles angles;

    // 6 key points of the face
    public List<Point2f> points = new List<Point2f>(6);
    // new points after reflecting
    public List<Point2f> new2Dpoints;


    public void updatePoints(FaceLandmarks landmarks)
    {
        //points = now_points;
        //points = landmarks.points.ConvertAll(point => new Point2f(point.x, point.y));
        // Tipnose
        points.Add(new Point2f(landmarks.points[30].x, landmarks.points[30].y));
        // Chin
        points.Add(new Point2f(landmarks.points[8].x, landmarks.points[8].y));
        // Left corner of the left eye
        points.Add(new Point2f(landmarks.points[36].x, landmarks.points[36].y));
        // Right corner of the right eye
        points.Add(new Point2f(landmarks.points[45].x, landmarks.points[45].y));
        // Left corner of the mouth
        points.Add(new Point2f(landmarks.points[48].x, landmarks.points[48].y));
        // Right corner of the mouth
        points.Add(new Point2f(landmarks.points[54].x, landmarks.points[54].y));
    }

    public Mat GetAnglesAndPoints(IEnumerable<Point3f> originalPoints)
    {
        var rvec = new double[] { 0, 0, 0 };
        var tvec = new double[] { 0, 0, 0 };
        var cameraMatrix = new double[3, 3]
        {
            { 1,0,0},
            { 0,1,0},
            { 0,0,1}
        };

        var dist = new double[] { 0, 0, 0, 0, 0 };
        var objPts = new[] { originalPoints };
        var imgPts = new[] { this.points };
        // store the 2D reflected points
        Mat resultPoints = new Mat();

        //calculate the pose
        using (var objPtsMat = new Mat(objPts.Length, 1, MatType.CV_32FC3, objPts))
        using (var imgPtsMat = new Mat(imgPts.Length, 1, MatType.CV_32FC2, imgPts))
        using (var cameraMatrixMat = Mat.Eye(3, 3, MatType.CV_64FC1))
        using (var distMat = Mat.Zeros(5, 0, MatType.CV_64FC1))
        using (var rvecMat = new Mat())
        using (var tvecMat = new Mat())
        {
            // solve for pose
            Cv2.SolvePnP(objPtsMat, imgPtsMat, cameraMatrixMat, distMat, rvecMat, tvecMat);

            // draw the 3D points to 2D image plane
            Cv2.ProjectPoints(objPtsMat, rvecMat, tvecMat, cameraMatrixMat, distMat, resultPoints);
            //Debug.Log(resultPoints);

            // 根据旋转矩阵求解坐标旋转角
            double theta_x = Mathf.Atan2((float)rvecMat.At<double>(2, 1), (float)rvecMat.At<double>(2, 2));
            double theta_y = Mathf.Atan2((float)(-rvecMat.At<double>(2, 0)),
                 (Mathf.Sqrt((float)(rvecMat.At<double>(2, 1) * rvecMat.At<double>(2, 1) + (float)rvecMat.At<double>(2, 2) * rvecMat.At<double>(2, 2)))));
            double theta_z = Mathf.Atan2((float)rvecMat.At<double>(1, 0), (float)rvecMat.At<double>(0, 0));

            // 将弧度转为角度
            this.angles.roll = theta_x * (180 / Mathf.PI);
            this.angles.pitch = theta_y * (180 / Mathf.PI);
            this.angles.yaw = theta_z * (180 / Mathf.PI);

            // 将映射的点的坐标保存下来
            // outarray类型的resultpoints如何转存到list中？
            return resultPoints;
        }
    }

   
}
