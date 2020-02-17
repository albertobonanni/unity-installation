using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public string animSaveToFile = "Assets/AzureKinectExamples/KinectDemos/MocapAnimatorDemo/Animations/Recorded.anim";
    uint fileId = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            print(fileId.ToString("D4"));
            
            string animSaveToFileNu = "";

            if (animSaveToFile.EndsWith(".anim"))
            {
                animSaveToFileNu = animSaveToFile.Insert(animSaveToFile.Length - 5, fileId.ToString("D4"));
                print(animSaveToFileNu);
            }
                

            int iP = animSaveToFileNu.LastIndexOf('/');
            string animName = (iP >= 0 ? animSaveToFileNu.Substring(iP + 1) : animSaveToFileNu).Trim();

            if (animName.EndsWith(".anim"))
                    animName = animName.Substring(0, animName.Length - 5);
            print("Name: " + animName);
            fileId++;
        }
    }
}
