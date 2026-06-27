using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueMarker : Marker, INotification
{
    public string text;

    public PropertyName id => new PropertyName("DialogueMarker");
}
