using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SongList", menuName = "ScriptableObjects/SongList", order = 1)]
public class SoSongList : ScriptableObject
{
    public List<SongData> songList;
    public List<AudioClip> clips;
    public string[] removeNames;

    [ContextMenu("AddSong")]
    public void AddSong()
    {
        songList.Clear();
        foreach (AudioClip file in clips)
        {
            string cleanName = file.name;

            foreach (string remove in removeNames)
            {
                if (!string.IsNullOrEmpty(remove))
                {
                    cleanName = cleanName.Replace(remove, "");
                }
            }

            songList.Add(new SongData()
            {
                name = cleanName,
                clip = file
            });
        }
    }
}

[System.Serializable]
public class SongData
{
    public string name;
    public AudioClip clip;
}