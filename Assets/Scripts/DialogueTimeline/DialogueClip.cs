using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Clip: PlayerAsset 在轨道上作为Asset资源片段
/// </summary>
public class DialogueClip : PlayableAsset, ITimelineClipAsset
{
    // 作为模板实例传给工厂方法
    public DialogueBehaviour template = new DialogueBehaviour();

    // 片段混合方式：不能混合
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // 工厂方法创建Playable
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph, template);
        return playable;
    }
}
