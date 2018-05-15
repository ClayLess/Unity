using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using LeapSql;

namespace Assets
{
    public class AddHand
    {
        public static string ToFrame(ref Frame frame,Hand _addHand)
        {

            string s = "";
            Hand copy = new Hand();
            copy.CopyFrom(_addHand);
            //s+=_addHand.Id+" "+_addHand.TimeVisible+" "+_addHand.WristPosition.x+" ";
            if (frame.Hands.Count < 2)
            {
                if (frame.Hands.Count == 1)
                {
                    if(frame.Hands[0].Id!=1)
                    frame.Hands.Insert(0, copy);
                }
                else
                {
                    frame.Hands.Add(copy);
                }
            }
            foreach(Hand h in frame.Hands)
            {
                s += h.Id + " ";
            }
            return frame.Id+" "+frame.Hands.Count+" "+s;
            
        }
        public static Hand getHand()
        {
            //Bones setting
            Bone bone00 = new Bone(new Vector(-55, 226, 59), new Vector(-55, 226, 59), new Vector(-55, 226, 59), new Vector(0, 0, 0), 0, (float)16.8, Bone.BoneType.TYPE_METACARPAL, new LeapQuaternion((float)0.1993, (float)-0.2273, (float)-0.4926, (float)0.8160));
            Bone bone01 = new Bone(new Vector(-55, 226, 59), new Vector(-23, 226, 27), new Vector(-39, 226, 43), new Vector((float)0.7, (float)-0.018, (float)-0.7131), 45, (float)16.8, Bone.BoneType.TYPE_PROXIMAL, new LeapQuaternion((float)0.1934, (float)-0.3256, (float)-0.492954, (float)0.7832813));
            Bone bone02 = new Bone(new Vector(-23, 226, 27), new Vector(-4, 224, 4), new Vector(-13, 225, 16), new Vector((float)0.6434, (float)-0.553, (float)-0.7635), 29, (float)16.8, Bone.BoneType.TYPE_INTERMEDIATE, new LeapQuaternion((float)0.1599, (float)-0.3044, (float)-0.5064, (float)0.7908));
            Bone bone03 = new Bone(new Vector(-4, 224, 4), new Vector(9, 223, -10), new Vector(3, (float)223.5, -3), new Vector((float)0.6689, (float)-0.0392, (float)0.7423), 20, (float)16.8, Bone.BoneType.TYPE_DISTAL, new LeapQuaternion((float)0.1744, (float)-0.3136, (float)-0.5, (float)0.7877));
            Bone bone10 = new Bone(new Vector(-61, 245, 50), new Vector(-40, 241, -13), new Vector(-51, 243, 19), new Vector((float)0.3191, (float)-0.0484, (float)-0.9865), 67, 16, Bone.BoneType.TYPE_METACARPAL, new LeapQuaternion((float)-0.253, (float)-0.1616, (float)0.0049, (float)0.9865));
            Bone bone11 = new Bone(new Vector(-40, 241, -13), new Vector(-29, 242, -48), new Vector(-34, 242, -30), new Vector((float)0.3016, (float)0.03188, (float)-0.9529), 37, 16, Bone.BoneType.TYPE_PROXIMAL, new LeapQuaternion((float)0.01446, (float)-0.1528, (float)0.0108, (float)0.988));
            Bone bone12 = new Bone(new Vector(-29, 242, -48), new Vector(-22, 239, -68), new Vector(-26, 241, -58), new Vector((float)0.3, (float)-0.1816, (float)-0.9364), 21, 16, Bone.BoneType.TYPE_INTERMEDIATE, new LeapQuaternion((float)-0.0914, (float)-0.1513, (float)-0.0056, (float)0.9839));
            Bone bone13 = new Bone(new Vector(-22, 239, -68), new Vector(-18, 233, -81), new Vector(-20, 236, -74), new Vector((float)0.2847, (float)-0.3889, (float)-0.8761), 15, 16, Bone.BoneType.TYPE_DISTAL, new LeapQuaternion((float)-0.1973, (float)-0.1515, (float)-0.221, (float)0.9683));
            Bone bone20 = new Bone(new Vector(-72, 246, 46), new Vector(-60, 243, -16), new Vector(-66, 244, 15), new Vector((float)0.1864, (float)-0.0517, (float)-0.9811), (float)63.8, 16, Bone.BoneType.TYPE_METACARPAL, new LeapQuaternion((float)-0.0341, (float)-0.0909, (float)0.0882, (float)0.9913));
            Bone bone21 = new Bone(new Vector(-60, 243, -16), new Vector(-55, 244, -57), new Vector(-57, 243, -36), new Vector((float)0.1227, (float)0.0352, (float)-0.9918), 42, 16, Bone.BoneType.TYPE_PROXIMAL, new LeapQuaternion((float)0.0120, (float)-0.0628, (float)0.0905, (float)0.9938));
            Bone bone22 = new Bone(new Vector(-55, 244, -57), new Vector(-51, 240, -81), new Vector(-53, 242, -69), new Vector((float)0.1576, (float)-0.1694, (float)-0.9729), 24, 16, Bone.BoneType.TYPE_INTERMEDIATE, new LeapQuaternion((float)-0.0916, (float)-0.0719, (float)0.0834, (float)0.9897));
            Bone bone23 = new Bone(new Vector(-51, 240, -81), new Vector(-48, 234, -96), new Vector(-49, 237, -89), new Vector((float)0.19, (float)-0.4015, (float)-0.8959), 16, 16, Bone.BoneType.TYPE_DISTAL, new LeapQuaternion((float)-0.213, (float)-0.0816, (float)0.074, (float)0.9708));
            Bone bone30 = new Bone(new Vector(-83, 245, 43), new Vector(-80, 241, -13), new Vector(-81, 243, 15), new Vector((float)0.0364, (float)-0.0516, (float)-0.9976), 57, 15, Bone.BoneType.TYPE_METACARPAL, new LeapQuaternion((float)-0.0308, (float)-0.0145, (float)0.123, (float)0.9918));
            Bone bone31 = new Bone(new Vector(-80, 241, -13), new Vector(-82, 244, -52), new Vector(-81, 242, -32), new Vector((float)-0.0418, (float)0.052, (float)-0.9978), 38, 15, Bone.BoneType.TYPE_PROXIMAL, new LeapQuaternion((float)0.283, (float)0.0175, (float)0.1214, (float)0.9920));
            Bone bone32 = new Bone(new Vector(-82, 243, -52), new Vector(-82, 240, -75), new Vector(-82, 242, -64), new Vector((float)0.0061, (float)-0.1392, (float)-0.9902), 24, 15, Bone.BoneType.TYPE_INTERMEDIATE, new LeapQuaternion((float)-0.0696, (float)0.0055, (float)0.1225, (float)0.99));
            Bone bone33 = new Bone(new Vector(-82, 240, -75), new Vector(-81, 234, -90), new Vector(-81, 237, -83), new Vector((float)0.0643, (float)-0.3654, (float)-0.9285), 16, 15, Bone.BoneType.TYPE_DISTAL, new LeapQuaternion((float)-0.1887, (float)-0.0093, (float)0.1223, (float)0.9743));
            Bone bone40 = new Bone(new Vector(-93, 237, 43), new Vector(-99, 236, -9), new Vector(-96, 237, 17), new Vector((float)-0.1052, (float)-0.0221, (float)-0.9942), 53, 13, Bone.BoneType.TYPE_METACARPAL, new LeapQuaternion((float)-0, (float)0.0538, (float)0.201, (float)0.9781));
            Bone bone41 = new Bone(new Vector(-99, 236, -9), new Vector(-107, 237, -38), new Vector(-103, 237, -24), new Vector((float)-0.2763, (float)0.0459, (float)-0.9599), 30, 13, Bone.BoneType.TYPE_PROXIMAL, new LeapQuaternion((float)0.0499, (float)0.1324, (float)0.1923, (float)0.97108));
            Bone bone42 = new Bone(new Vector(-107, 237, -38), new Vector(-111, 235, -55), new Vector(-109, 236, -46), new Vector((float)-0.2007, (float)-0.1347, (float)-0.9703), 17, 13, Bone.BoneType.TYPE_INTERMEDIATE, new LeapQuaternion((float)-0.045, (float)0.1129, (float)0.2043, (float)0.9713));
            Bone bone43 = new Bone(new Vector(-111, 235, -55), new Vector(-112, 230, -69), new Vector(-111, 233, -62), new Vector((float)-0.0938, (float)-0.356, (float)-0.9297), 15, 13, Bone.BoneType.TYPE_DISTAL, new LeapQuaternion((float)-0.1663, (float)-0.0866, (float)0.2168, (float)0.9580));

            //Fingers seetting
            List<Finger> fingers = new List<Finger>();

            fingers.Add(new Finger(1, 11, 110, 10000, new Vector(9, 223, -10), new Vector(-132, -101, 2), new Vector((float)0.6434, (float)-0.0552, (float)-0.7635), new Vector(5, 220, -7), 17, 90, true, Finger.FingerType.TYPE_THUMB, bone00, bone01, bone02, bone03));
            fingers.Add(new Finger(1, 11, 111, 10000, new Vector(-18, 223, -81), new Vector(-132, -101, 2), new Vector((float)0.3001, (float)-0.181, (float)-0.936), new Vector(-22, 236, -80), 16, 70, true, Finger.FingerType.TYPE_INDEX, bone10, bone11, bone12, bone13));
            fingers.Add(new Finger(1, 11, 112, 10000, new Vector(-48, 233, -96), new Vector(-128, -32, 26), new Vector((float)0.1576, (float)-0.1694, (float)-0.9728), new Vector(-50, 236, -93), 16, 78, true, Finger.FingerType.TYPE_MIDDLE, bone20, bone21, bone22, bone23));
            fingers.Add(new Finger(1, 11, 113, 10000, new Vector(-81, 234, -90), new Vector(-118, -56, 35), new Vector((float)0.0061, (float)-0.1392, (float)-0.9902), new Vector(-80, 233, -87), 15, 75, true, Finger.FingerType.TYPE_RING, bone30, bone31, bone32, bone33));
            fingers.Add(new Finger(1, 11, 114, 10000, new Vector(-112, 230, -69), new Vector(-101, -64, 34), new Vector((float)-0.2007, (float)-0.1347, (float)-0.9703), new Vector(-110, 229, -65), 13, 59, true, Finger.FingerType.TYPE_PINKY, bone40, bone41, bone42, bone43));


            //Hand setting
            Arm arm = new Arm(new Vector(-107, 177, 312), new Vector(-83, 232, 72), new Vector(-95, 204, 192), new Vector((float)0.09857, (float)0.2244, (float)-0.9694), (float)247, (float)54, new LeapQuaternion((float)0.1116, (float)-0.0528, (float)0.0284, (float)0.9919));

            Hand hand = new Hand(1, 11, 1, 0, (float)0.334, 0, (float)61.773, 83, true, 10000, arm, fingers, new Vector(-66, 236, 5), new Vector(-58, 239, 11), new Vector(-110, -50, 20), new Vector((float)0.0487, (float)-0.9947, (float)-0.0894), new LeapQuaternion((float)0.0465, (float)-0.0771, (float)0.0208, (float)0.9957), new Vector((float)0.1517, (float)0.0959, (float)-0.9837), new Vector(-83, 232, 72));



            return hand;
        }
        //从数据库提取手型
        public static Hand getHand(int id, string connectinfo)
        {
            UnityEngine.Debug.Log(id+","+connectinfo);
            //static test
            HandSql hs = new HandSql(new MySql.Data.MySqlClient.MySqlConnection(connectinfo));
            hs.GetHandFromDB(id);
            return hs.hand;
        }
    }
}
