using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebCamFaceLandmarkDetector;
using OpenCvSharp;


namespace FaceToFaceManager
{

	public class FaceToFace : MonoBehaviour
	{
		public AnimManager animManager;

		private FaceLandmarks originalLandmarks = null;

		private FaceLandmarks currentLandmarks = null;

		private FacialValues lastFacialValues = new FacialValues();

        private FacePosture currentPosture = null;

        private List<Point3f> model_points = new List<Point3f>()
        {
            // tip nose
            new Point3f(0.0f,0.0f,0.0f),
            // chain
            new Point3f(0.0f,-330.0f,-65.0f),
            // left corner of left eye
            new Point3f(-225.0f,170.0f,-135.0f),
            // right corner of right eye
            new Point3f(225.0f,170.0f,-135.0f),
            // left corner of mouth
            new Point3f(-150.0f,-150.0f,-125.0f),
            // right corner of mouth
            new Point3f(150.0f,-150.0f,-125.0f)
        };
        
        // Start is called before the first frame update
        void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		private void LateUpdate()
		{
			UpdateFacial();
		}

		public void InitFace()
		{
			originalLandmarks = null;

			currentLandmarks = null;
            
            currentPosture = null;
		}
        
       
		public void UpdateFacial(FaceLandmarks landmarks)
		{
			if (originalLandmarks == null)
            {
                originalLandmarks = landmarks;
                lastFacialValues.initial();
            }
			else
            { 
				currentLandmarks = landmarks;

                //GetAngles(currentLandmarks);

            }
        }

		public void UpdateFacial()
		{
			FacialValues currentFacialValues = new FacialValues();

            if (currentLandmarks != null)
			{
				//Vector2 top = getTopDir(landmarks);

				currentFacialValues.Eye_L_Closed = GetEyeClosedValue(currentLandmarks, FaceElement.leftEye);
				currentFacialValues.Eye_R_Closed = GetEyeClosedValue(currentLandmarks, FaceElement.rightEye);

                //其他表情状态的匹配
                currentFacialValues.Eye_L_Wide = GetEyeWideValue(currentLandmarks, FaceElement.leftEye);
                currentFacialValues.Eye_R_Wide = GetEyeWideValue(currentLandmarks, FaceElement.rightEye);

                currentFacialValues.Eyebrow_L_Sad = GetEyebowSadValue(currentLandmarks, FaceElement.leftEyeBrow);
                currentFacialValues.Eyebrow_R_Sad = GetEyebowSadValue(currentLandmarks, FaceElement.rightEyeBrow);

                currentFacialValues.Eyebrow_L_Angry = GetEyebowAngryValue(currentLandmarks, FaceElement.leftEyeBrow);
                currentFacialValues.Eyebrow_R_Angry = GetEyebowAngryValue(currentLandmarks, FaceElement.rightEyeBrow);

                currentFacialValues.Eyebrow_L_Up = GetEyebowUpValue(currentLandmarks, FaceElement.leftEyeBrow);
                currentFacialValues.Eyebrow_R_Up = GetEyebowUpValue(currentLandmarks, FaceElement.rightEyeBrow);

                currentFacialValues.Mouth_O = GetMouthOpenValue(currentLandmarks, FaceElement.mouth);

                currentFacialValues.Mouth_Puff = GetMouthPuffValue(currentLandmarks, FaceElement.outline);

                currentFacialValues.Mouth_Tongue = GetTongue(currentLandmarks, FaceElement.mouth);
            }

            currentFacialValues = (currentFacialValues * Config.motionBlendCoeffience + lastFacialValues) / (Config.motionBlendCoeffience + 1);

			animManager.SetFacial(currentFacialValues * 100);
			lastFacialValues = currentFacialValues;

			if(currentLandmarks!=null)
				GetAngles(currentLandmarks);
        }

        private void GetAngles(FaceLandmarks currentLandmarks)
        {
            currentPosture = new FacePosture();
            currentPosture.updatePoints(currentLandmarks);
            currentPosture.GetAnglesAndPoints(model_points);
            print("roll:" + currentPosture.angles.roll + " yaw:" + currentPosture.angles.yaw + " pitch:" + currentPosture.angles.pitch);
        } 
        
		private float GetEyeClosedValue(FaceLandmarks landmarks, FaceElement eye)
		{
			if (eye != FaceElement.leftEye && eye != FaceElement.rightEye)
#if DEBUG
				throw new System.Exception();
#else
				return 0F;
#endif
            // 尺度扩大三倍，便于显示
            return Mathf.Clamp01((1 - GetEyeOpenSize(landmarks.getElement(eye)) / GetEyeOpenSize(originalLandmarks.getElement(eye)))*3);
		}

        // 上眼皮挑动的比例,以初始睁眼尺度的1/10为标准
        private float GetEyeWideValue(FaceLandmarks landmarks, FaceElement eye)
        {
            if (eye != FaceElement.leftEye && eye != FaceElement.rightEye)
#if DEBUG
                throw new System.Exception();
#else
                return 0F;
#endif
            return Mathf.Clamp01(GetEyeWideSize(landmarks.getElement(eye))/(GetEyeOpenSize(originalLandmarks.getElement(eye))/10));
        }

        // 悲伤时的眉尾下滑比例
        private float GetEyebowSadValue(FaceLandmarks landmarks, FaceElement eyebow)
        {
            if (eyebow != FaceElement.leftEyeBrow && eyebow != FaceElement.rightEyeBrow)
#if DEBUG
                throw new System.Exception();
#else
            return 0F;
#endif

            List<Vector2> original = originalLandmarks.getElement(eyebow);
            List<Vector2> current = landmarks.getElement(eyebow);
            Vector2 vertial = Vector2.Perpendicular((original[0] - original[4])).normalized;
            Vector2 distance = current[0] - current[4];
            
            return Mathf.Clamp01(Vector2.Dot(distance, vertial));
        }

        // 愤怒时的眉毛下滑比例
        private float GetEyebowAngryValue(FaceLandmarks landmarks, FaceElement eyebow)
        {
            if (eyebow != FaceElement.leftEyeBrow && eyebow != FaceElement.rightEyeBrow)
#if DEBUG
                throw new System.Exception();
#else
            return 0F;
#endif

            List<Vector2> original = originalLandmarks.getElement(eyebow);
            List<Vector2> current = landmarks.getElement(eyebow);
            Vector2 vertial = Vector2.Perpendicular((original[0] - original[4])).normalized;

            Vector2 distance = current[0] - current[0] + current[4] - current[4];
            
            return Mathf.Clamp01(Vector2.Dot(distance, vertial));
        }
        
        // 眉毛整体上移比例
        private float GetEyebowUpValue(FaceLandmarks landmarks, FaceElement eyebow)
        {
            if (eyebow != FaceElement.leftEyeBrow && eyebow != FaceElement.rightEyeBrow)
#if DEBUG
                throw new System.Exception();
#else
            return 0F;
#endif
            
            List<Vector2> oriEyebow = originalLandmarks.getElement(eyebow);
            List<Vector2> curEyebow = landmarks.getElement(eyebow);
            Vector2 original = oriEyebow[4] - oriEyebow[0];
            Vector2 current = curEyebow[4] - curEyebow[0];
            
            return Mathf.Clamp01(Vector2.Distance(current,original) / GetEyeOpenSize(originalLandmarks.points.GetRange(36,6)));

        }

        // 张嘴比例
        private float GetMouthOpenValue(FaceLandmarks landmarks, FaceElement mouth)
        {
            if (mouth != FaceElement.mouth)
#if DEBUG
                throw new System.Exception();
#else
            return 0F;
#endif
            return Mathf.Clamp01(GetMouthOpenSize(landmarks.getElement(mouth)));
        }

        // 嘟嘴比例
        private float GetMouthPuffValue(FaceLandmarks landmarks, FaceElement outline)
        {
            if (outline != FaceElement.outline)
#if DEBUG
                throw new System.Exception();
#else
            return 0F;
#endif
            Vector2 original = originalLandmarks.points[12] - originalLandmarks.points[4];
            Vector2 current = landmarks.points[12] - landmarks.points[4];

            float o_dist = original.magnitude;
            float c_dist = current.magnitude;
            return Mathf.Clamp01((c_dist/o_dist-1)*8);
            
        }

        // 是否吐舌头
        private bool GetTongue(FaceLandmarks landmarks, FaceElement mouth)
        {
            if (mouth != FaceElement.mouth)
#if DEBUG
                throw new System.Exception();
#else
            return false;
#endif

            Vector2 vertical = originalLandmarks.points[48] - originalLandmarks.points[54];
            Vector2 distance = landmarks.points[48] - landmarks.points[54];
            
            if (Vector2.Angle(vertical, distance)>4.5)
                return true;
            return false;
             
        }

        private float GetMouthOpenSize(List<Vector2> mouthLandmarks)
        {
            Vector2 vertical = Vector2.Perpendicular((mouthLandmarks[16] - mouthLandmarks[12])).normalized;
            Vector2 distance = (mouthLandmarks[19] + mouthLandmarks[17] - mouthLandmarks[13] - mouthLandmarks[15]) / 2;
            
            return Vector2.Dot(distance, vertical);
        }
        private float GetEyeWideSize(List<Vector2> eyeLandmarks)
        {
            Vector2 vertial = Vector2.Perpendicular((eyeLandmarks[3] - eyeLandmarks[0])).normalized;
            Vector2 distance = (eyeLandmarks[1] + eyeLandmarks[2] - originalLandmarks.points[37] - originalLandmarks.points[38]) / 2;

            return Vector2.Dot(distance, vertial);
        }

		private float GetEyeOpenSize(List<Vector2> eyeLandmarks)
		{
            
			Vector2 vertical = Vector2.Perpendicular((eyeLandmarks[3] - eyeLandmarks[0])).normalized;

			Vector2 distance = (eyeLandmarks[1] + eyeLandmarks[2] - eyeLandmarks[4] - eyeLandmarks[5]) / 2;


            return Vector2.Dot(distance, vertical);
		}

       

	}
}

