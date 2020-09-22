//Use this type of look at if you are using a recent Unity version

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] 
[ExecuteInEditMode]
public class IK_PointAt : MonoBehaviour {
    
    protected Animator animator;

    public string pointLayerName = "point";
    private int pointLayerIndex = -1;

    
    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;
    [Range(0, 1)]
    public float weight = 0;
#if UNITY_EDITOR    
    public bool updateInEditor = false;
#endif


    void Start () 
    {
        animator = GetComponent<Animator>();

        Debug.Log( animator.layerCount );
        for(int i = 0; i < animator.layerCount; i++ ){
            Debug.Log("animation layer " + i + " " + animator.GetLayerName(i) );//GetLayerName(int layerIndex);
            if( pointLayerName == animator.GetLayerName(i)){
                pointLayerIndex = i;
            }
        }
        if( pointLayerIndex != -1){
            animator.SetLayerWeight(pointLayerIndex, weight);
        }
    }
    
    //a callback for calculating IK - this example handles head look and a dynamic right hand
    void OnAnimatorIK()
    {

        if(animator) {
            
            //if the IK is active, set the position and rotation directly to the goal. 
            if(ikActive && weight > 0.01f) {

                // Set the look target position, if one has been assigned
                if(lookObj != null) {

                    //SetLookAtWeight(float weight, float bodyWeight = 0.0f, float headWeight = 1.0f, float eyesWeight = 0.0f, float clampWeight = 0.5f);
                    animator.SetLookAtWeight(weight, weight * 0.25f, weight * 0.75f);
                    animator.SetLookAtPosition(lookObj.position);
                }

                if( pointLayerIndex != -1){
                    animator.SetLayerWeight(pointLayerIndex, weight);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if(rightHandObj != null) {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand,weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand,weight);  
                    animator.SetIKPosition(AvatarIKGoal.RightHand,rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand,rightHandObj.rotation);
                }        
                
            }
            
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {          
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
                animator.SetLookAtWeight(0);
                animator.SetLayerWeight(pointLayerIndex, 0);
            }
        }
    }  
#if UNITY_EDITOR
    void Update(){
        if(updateInEditor){
            animator.Update(0);
        }
    }
#endif
}

