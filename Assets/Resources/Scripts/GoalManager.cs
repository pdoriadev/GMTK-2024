using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] List<Goal> goals = new List<Goal>();
    string[] _celebrationSfx = { "Celebration-1" };

    bool hasWon = false;

    private void Update()
    {
        if (hasWon == false && HaveWon())
        {
            AudioPlayer.Instance.SoundEffect(_celebrationSfx[0]);
        }
    }

    private bool HaveWon()
    {
        foreach (Goal g in goals)
        {
            if (g.count < g.goal)
            {
                return false;
            }
        }

        hasWon = true;
        return true;
    }
}
