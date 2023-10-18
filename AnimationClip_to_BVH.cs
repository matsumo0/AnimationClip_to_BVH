using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // Unityエディター上でしか動きません
using UnityEditor.Animations; // Unityエディター上でしか動きません


[ RequireComponent( typeof( BVHRecorder ) ) ] // BVHレコーダー必須

public class AnimationClip_to_BVH : MonoBehaviour
{

    [ Header( "使い方：\n・アニメーションクリップを設定する\n・収録フレームレートを入力する\n・保存場所を選択する\n・ファイル名を入力する\n・「設定を適用する」ボタンを押して適用する\n・「再生する」ボタンを押してプレイモードに入る\n→ 自動的に1アニメーション分収録されプレイモードが終了します。" ) ]

    [ Space ]

    [ Header( "アニメーションクリップ" ) ]
    public AnimationClip animation_clip = null;
    // アニメーションの総フレーム数
    int animation_total_frame_num = 0;

    [ Space ]

    [ Header( "収録フレームレート\n下の「アニメーションクリップのフレームレートを取得する」ボタン\nからアニメーションオリジナルのフレームレートを取得することもできます。" ) ]
    public float frame_rate = 30f;

    [ Space ]

    [ Header( "BVHファイルを保存する場所（フォルダ）\n下の 「保存場所を選択する」 ボタンから選択できます。" ) ]
    public string save_folder_path = "";
    [ Header( "保存するBVHファイルの名前" ) ]
    public string save_file_name = "";

    [ Space ]

    public Animator ava_animator = null;

    #region 各ボーンのトランスフォーム
    public Transform  hips_t, spine_t, chest_t, upperChest_t, neck_t, // 背骨のボーンのトランスフォーム
                      leftUpperLeg_t, leftLowerLeg_t, leftFoot_t, leftToes_t, // 左脚のボーンのトランスフォーム
                      rightUpperLeg_t, rightLowerLeg_t, rightFoot_t, rightToes_t, // 右脚のボーンのトランスフォーム
                      leftShoulder_t, leftUpperArm_t, leftLowerArm_t, // 左腕のボーンのトランスフォーム
                      leftHand_t, // 左手のボーンのトランスフォーム
                      leftThumbProximal_t, leftThumbIntermediate_t, leftThumbDistal_t,
                      leftIndexProximal_t, leftIndexIntermediate_t, leftIndexDistal_t,
                      leftMiddleProximal_t, leftMiddleIntermediate_t, leftMiddleDistal_t,
                      leftRingProximal_t, leftRingIntermediate_t, leftRingDistal_t,
                      leftLittleProximal_t, leftLittleIntermediate_t, leftLittleDistal_t,
                      rightShoulder_t, rightUpperArm_t, rightLowerArm_t, // 右腕のボーンのトランスフォーム
                      rightHand_t, // 右手のボーンのトランスフォーム
                      rightThumbProximal_t, rightThumbIntermediate_t, rightThumbDistal_t,
                      rightIndexProximal_t, rightIndexIntermediate_t, rightIndexDistal_t,
                      rightMiddleProximal_t, rightMiddleIntermediate_t, rightMiddleDistal_t,
                      rightRingProximal_t, rightRingIntermediate_t, rightRingDistal_t,
                      rightLittleProximal_t, rightLittleIntermediate_t, rightLittleDistal_t,
                      head_t, leftEye_t, rightEye_t; // 頭・左目・右目のボーン
    #endregion

    public BVHRecorder bvh_recorder = null;

    AnimatorController animator_controller = null;

    // このスクリプトをアタッチした時に実行される関数
    void Reset(){
		ava_animator = GetComponent<Animator>();

        #region 各ボーンのトランスフォームの取得（必須でないボーンが存在しなかった場合はnullが入る）
        hips_t = ava_animator.GetBoneTransform( HumanBodyBones.Hips );
        spine_t = ava_animator.GetBoneTransform( HumanBodyBones.Spine );
        chest_t = ava_animator.GetBoneTransform( HumanBodyBones.Chest );
        upperChest_t = ava_animator.GetBoneTransform( HumanBodyBones.UpperChest ); // 必須でないボーン
        neck_t = ava_animator.GetBoneTransform( HumanBodyBones.Neck );
        leftUpperLeg_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftUpperLeg );
        leftLowerLeg_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftLowerLeg );
        leftFoot_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftFoot );
        leftToes_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftToes ); // 必須でないボーン
        rightUpperLeg_t = ava_animator.GetBoneTransform( HumanBodyBones.RightUpperLeg );
        rightLowerLeg_t = ava_animator.GetBoneTransform( HumanBodyBones.RightLowerLeg );
        rightFoot_t = ava_animator.GetBoneTransform( HumanBodyBones.RightFoot );
        rightToes_t = ava_animator.GetBoneTransform( HumanBodyBones.RightToes ); // 必須でないボーン
        leftShoulder_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftShoulder ); // 必須でないボーン
        leftUpperArm_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftUpperArm );
        leftLowerArm_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftLowerArm );
        leftHand_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftHand );
        leftThumbProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftThumbProximal ); // 必須でないボーン
        leftThumbIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftThumbIntermediate ); // 必須でないボーン
        leftThumbDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftThumbDistal ); // 必須でないボーン
        leftIndexProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftIndexProximal ); // 必須でないボーン
        leftIndexIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftIndexIntermediate ); // 必須でないボーン
        leftIndexDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftIndexDistal ); // 必須でないボーン
        leftMiddleProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftMiddleProximal ); // 必須でないボーン
        leftMiddleIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftMiddleIntermediate ); // 必須でないボーン
        leftMiddleDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftMiddleDistal ); // 必須でないボーン
        leftRingProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftRingProximal ); // 必須でないボーン
        leftRingIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftRingIntermediate ); // 必須でないボーン
        leftRingDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftRingDistal ); // 必須でないボーン
        leftLittleProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftLittleProximal ); // 必須でないボーン
        leftLittleIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftLittleIntermediate ); // 必須でないボーン
        leftLittleDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftLittleDistal ); // 必須でないボーン
        rightShoulder_t = ava_animator.GetBoneTransform( HumanBodyBones.RightShoulder ); // 必須でないボーン
        rightUpperArm_t = ava_animator.GetBoneTransform( HumanBodyBones.RightUpperArm );
        rightLowerArm_t = ava_animator.GetBoneTransform( HumanBodyBones.RightLowerArm );
        rightHand_t = ava_animator.GetBoneTransform( HumanBodyBones.RightHand );
        rightThumbProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightThumbProximal ); // 必須でないボーン
        rightThumbIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.RightThumbIntermediate ); // 必須でないボーン
        rightThumbDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightThumbDistal ); // 必須でないボーン
        rightIndexProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightIndexProximal ); // 必須でないボーン
        rightIndexIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.RightIndexIntermediate ); // 必須でないボーン
        rightIndexDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightIndexDistal ); // 必須でないボーン
        rightMiddleProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightMiddleProximal ); // 必須でないボーン
        rightMiddleIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.RightMiddleIntermediate ); // 必須でないボーン
        rightMiddleDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightMiddleDistal ); // 必須でないボーン
        rightRingProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightRingProximal ); // 必須でないボーン
        rightRingIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.RightRingIntermediate ); // 必須でないボーン
        rightRingDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightRingDistal ); // 必須でないボーン
        rightLittleProximal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightLittleProximal ); // 必須でないボーン
        rightLittleIntermediate_t = ava_animator.GetBoneTransform( HumanBodyBones.RightLittleIntermediate ); // 必須でないボーン
        rightLittleDistal_t = ava_animator.GetBoneTransform( HumanBodyBones.RightLittleDistal ); // 必須でないボーン
        head_t = ava_animator.GetBoneTransform( HumanBodyBones.Head );
        leftEye_t = ava_animator.GetBoneTransform( HumanBodyBones.LeftEye ); // 必須でないボーン
        rightEye_t = ava_animator.GetBoneTransform( HumanBodyBones.RightEye ); // 必須でないボーン
        #endregion

        bvh_recorder = GetComponent<BVHRecorder>();
	}

    void Start(){
        animation_total_frame_num = ( int ) Mathf.Round( animation_clip.length * animation_clip.frameRate );
        animator_controller = new AnimatorController();
        animator_controller.AddLayer( "Base Layer" );
        var state_machine = animator_controller.layers[0].stateMachine;
        var default_state = state_machine.AddState( "Default" );
        default_state.motion = animation_clip;
        ava_animator.runtimeAnimatorController = animator_controller;
        bvh_recorder.capturing = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if( bvh_recorder.capturing ){
            Debug.Log( "収録中です..." );

            if( animation_total_frame_num <= bvh_recorder.frameNumber ){
                bvh_recorder.capturing = false;
                bvh_recorder.saveBVH();
                EditorUtility.DisplayDialog( "収録を終了しました", "再生を停止します", "OK" );
                ava_animator.runtimeAnimatorController = null;
                animator_controller = null;
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }

    public void get_animation_frameRate(){
        frame_rate = animation_clip.frameRate;
    }
    public void select_folder(){
        save_folder_path = EditorUtility.OpenFolderPanel( "Open Folder", "", "" );
    }
    public void apply_settings(){
        // BVHレコーダーに必要な情報を渡す
        bvh_recorder.frameRate = frame_rate;
        bvh_recorder.directory = save_folder_path;
        bvh_recorder.filename = save_file_name;
        bvh_recorder.enforceHumanoidBones = true;
        bvh_recorder.targetAvatar = ava_animator;
        bvh_recorder.rootBone = hips_t;
        bvh_recorder.bones = new List<Transform>(){
                                hips_t,
                                spine_t,
                                chest_t,
                                neck_t,
                                leftUpperLeg_t,
                                leftLowerLeg_t,
                                leftFoot_t,
                                rightUpperLeg_t,
                                rightLowerLeg_t,
                                rightFoot_t,
                                leftUpperArm_t,
                                leftLowerArm_t,
                                leftHand_t,
                                rightUpperArm_t,
                                rightLowerArm_t,
                                rightHand_t,
                                head_t
                            };
        #region 必須でないボーンは存在する場合のみボーンリストに追加する
        if( upperChest_t != null ) bvh_recorder.bones.Add( upperChest_t );
        if( leftToes_t != null ) bvh_recorder.bones.Add( leftToes_t );
        if( rightToes_t != null ) bvh_recorder.bones.Add( rightToes_t );
        if( leftShoulder_t != null ) bvh_recorder.bones.Add( leftShoulder_t );
        if( leftThumbProximal_t != null ) bvh_recorder.bones.Add( leftThumbProximal_t );
        if( leftThumbIntermediate_t != null ) bvh_recorder.bones.Add( leftThumbIntermediate_t );
        if( leftThumbDistal_t != null ) bvh_recorder.bones.Add( leftThumbDistal_t );
        if( leftIndexProximal_t != null ) bvh_recorder.bones.Add( leftIndexProximal_t );
        if( leftIndexIntermediate_t != null ) bvh_recorder.bones.Add( leftIndexIntermediate_t );
        if( leftIndexDistal_t != null ) bvh_recorder.bones.Add( leftIndexDistal_t );
        if( leftMiddleProximal_t != null ) bvh_recorder.bones.Add( leftMiddleProximal_t );
        if( leftMiddleIntermediate_t != null ) bvh_recorder.bones.Add( leftMiddleIntermediate_t );
        if( leftMiddleDistal_t != null ) bvh_recorder.bones.Add( leftMiddleDistal_t );
        if( leftRingProximal_t != null ) bvh_recorder.bones.Add( leftRingProximal_t );
        if( leftRingIntermediate_t != null ) bvh_recorder.bones.Add( leftRingIntermediate_t );
        if( leftRingDistal_t != null ) bvh_recorder.bones.Add( leftRingDistal_t );
        if( leftLittleProximal_t != null ) bvh_recorder.bones.Add( leftLittleProximal_t );
        if( leftLittleIntermediate_t != null ) bvh_recorder.bones.Add( leftLittleIntermediate_t );
        if( leftLittleDistal_t != null ) bvh_recorder.bones.Add( leftLittleDistal_t );
        if( rightShoulder_t != null ) bvh_recorder.bones.Add( rightShoulder_t );
        if( rightThumbProximal_t != null ) bvh_recorder.bones.Add( rightThumbProximal_t );
        if( rightThumbIntermediate_t != null ) bvh_recorder.bones.Add( rightThumbIntermediate_t );
        if( rightThumbDistal_t != null ) bvh_recorder.bones.Add( rightThumbDistal_t );
        if( rightIndexProximal_t != null ) bvh_recorder.bones.Add( rightIndexProximal_t );
        if( rightIndexIntermediate_t != null ) bvh_recorder.bones.Add( rightIndexIntermediate_t );
        if( rightIndexDistal_t != null ) bvh_recorder.bones.Add( rightIndexDistal_t );
        if( rightMiddleProximal_t != null ) bvh_recorder.bones.Add( rightMiddleProximal_t );
        if( rightMiddleIntermediate_t != null ) bvh_recorder.bones.Add( rightMiddleIntermediate_t );
        if( rightMiddleDistal_t != null ) bvh_recorder.bones.Add( rightMiddleDistal_t );
        if( rightRingProximal_t != null ) bvh_recorder.bones.Add( rightRingProximal_t );
        if( rightRingIntermediate_t != null ) bvh_recorder.bones.Add( rightRingIntermediate_t );
        if( rightRingDistal_t != null ) bvh_recorder.bones.Add( rightRingDistal_t );
        if( rightLittleProximal_t != null ) bvh_recorder.bones.Add( rightLittleProximal_t );
        if( rightLittleIntermediate_t != null ) bvh_recorder.bones.Add( rightLittleIntermediate_t );
        if( rightLittleDistal_t != null ) bvh_recorder.bones.Add( rightLittleDistal_t );
        if( leftEye_t != null ) bvh_recorder.bones.Add( leftEye_t );
        if( rightEye_t != null ) bvh_recorder.bones.Add( rightEye_t );
        #endregion
    }
    public void play(){
        UnityEditor.EditorApplication.isPlaying = true;
    }

}


[ CustomEditor( typeof( AnimationClip_to_BVH ) ) ]
public class Animation_to_BVH_Editor : Editor{

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        AnimationClip_to_BVH anim_to_bvh = target as AnimationClip_to_BVH;

        if ( GUILayout.Button( "アニメーションクリップのフレームレートを取得する" ) ){
            anim_to_bvh.get_animation_frameRate();
        }

        if ( GUILayout.Button( "保存場所を選択する" ) ){
            anim_to_bvh.select_folder();
        }

        if ( GUILayout.Button( "設定を適用する" ) ){
            anim_to_bvh.apply_settings();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        }

        if( GUILayout.Button( "再生する" ) ){
            anim_to_bvh.play();
        }
    }

}