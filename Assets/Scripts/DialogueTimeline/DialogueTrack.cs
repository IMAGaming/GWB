using UnityEngine.Timeline;

/// <summary>
/// 定义对话轨
/// </summary>
[TrackColor(255/255f, 165/255f, 0f)]
[TrackClipType(typeof(DialogueClip))] // Track上放置的Clip的类型
[TrackBindingType(typeof(DialogueController))]
public class DialogueTrack : TrackAsset
{

}