using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

/// <summary>
/// Data:PlayableBehaviour 控制轨道的逻辑
/// </summary>
[System.Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public string characterName;
    [TextArea]public string dialogueLine;
    public Sprite characterHead;

    // 该片段是否开始播放
    private bool isClipPlayed;
    // 是否需要玩家响应
    public bool requirePause;
    // 暂停检测
    private bool pauseScheduled;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        DialogueController controller = playerData as DialogueController;

        // 如果当前片段权重大于0且片段未开始播放
        if (isClipPlayed == false && info.weight > 0)
        {
            // 设置相关数据
            controller.SetUpDialogueUI(characterName, dialogueLine, characterHead);
            isClipPlayed = true;
            if (requirePause)
                pauseScheduled = true;
        }
    
        float progress = (float)(playable.GetTime() / playable.GetDuration());
        controller.TextLineOnUpdate(dialogueLine, progress);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        isClipPlayed = false;
        if(pauseScheduled)
        {
            pauseScheduled = false;
            // 暂停Timeline播放
            TimelineMgr.Instance.PauseTimeline();
        }
    }
}
