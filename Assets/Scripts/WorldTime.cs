using UnityEngine;
using System.Collections;

public class WorldTime : MonoBehaviour
{
    public Light sun;
    public AudioClip roosterSound;
    private int day;
    private float dayTimeInGame;
    public static int SECONDS_PER_GAMEDAY = 60;
    public static int GAMEHOURS_PER_GAMEDAY = 24;
    public static int GAMEMINUTES_PER_GAMEHOUR = 60;
    public static int GAMESECONDS_PER_GAMEMINUTE = 60;
    private static float SECONDS_PER_GAMEHOUR = SECONDS_PER_GAMEDAY / GAMEHOURS_PER_GAMEDAY;
    private static float SECONDS_PER_GAMEMINUTE = SECONDS_PER_GAMEHOUR / GAMEMINUTES_PER_GAMEHOUR;
    private static float SECONDS_PER_GAMESECOND = SECONDS_PER_GAMEMINUTE / GAMESECONDS_PER_GAMEMINUTE;
    private static float DAWN = 6.0f;

    public Transform startPointP1;
    public Transform startPointP2;

    void Start ()
    {
        if (Mathf.Approximately (SECONDS_PER_GAMESECOND, 0.0f)) {
            Debug.LogError ("Seconds per Game Second is too small for a float! Adjust Game hours and minutes until it fits.");
            SECONDS_PER_GAMESECOND = 0.02f;
        }
        day = 0;
        SetGameTimeToHour (DAWN);
    }

    void Update ()
    {
        IncrementTimeInGame (Time.deltaTime);
    }

    /*
  * Increment the game time based on the supplied delta timestep
 */
    private void IncrementTimeInGame (float deltaTime)
    {
        SetGameTime (dayTimeInGame + ConvertSecondsToGameSeconds (deltaTime));

        // When we step over the time per day, snap to the next day.
        float gameSecondsInDay = (GAMESECONDS_PER_GAMEMINUTE * GAMEMINUTES_PER_GAMEHOUR * GAMEHOURS_PER_GAMEDAY);
        if (dayTimeInGame > gameSecondsInDay) {
            GoToNextDay ();
        }
    }

    /*
  * Snaps the enviornment and timers to the next day
 */
    public void GoToNextDay ()
    {
        AdvancePlants ();

        // Snap both players to start points
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            Transform startPoint;
            if(player.name == "Player1")
            {
                startPoint = startPointP1;
            }
            else
            {
                startPoint = startPointP2;
            }
            player.GetComponent<PlayerController>().SnapToPoint(startPoint);
        }

        AudioSource.PlayClipAtPoint (roosterSound, transform.position);

        day++;
        SetGameTimeToHour (DAWN);
    }

    private void AdvancePlants ()
    {
        GameObject[] plantObjs = GameObject.FindGameObjectsWithTag ("Plant");
        Plant plant = null;
        foreach (GameObject plantObj in plantObjs) {
            plant = (Plant)plantObj.GetComponent<Plant> ();
            plant.NightlyUpdate ();
        }
    }
 
    // Set the game time to the specified total second
    public void SetGameTime (float seconds)
    {
        dayTimeInGame = seconds;
     
        Sunlight sunScript = sun.GetComponent<Sunlight> ();
        sunScript.SetSunToTimeOfDay (GetDayTimeInHours ());
    }
 
    // Set the game time to the specified hour
    public void SetGameTimeToHour (float hours)
    {
        SetGameTime (hours * GAMESECONDS_PER_GAMEMINUTE * GAMEMINUTES_PER_GAMEHOUR);
    }    

    // Gets the current minute
    public float GetMinute ()
    {
        return GetDayTimeInMinutes () % GAMEMINUTES_PER_GAMEHOUR;
    }

    // Get the hour
    public float GetHour ()
    {
        return GetDayTimeInHours ();
    }

    // Get the day
    public float GetDay ()
    {
        return day;
    }

    // Returns the total Game Time passed in Game Minutes
    public float GetDayTimeInMinutes ()
    {
        return ConvertSecondsToGameMinutes (dayTimeInGame);
    }
 
    // Returns the total time for the day in Game Hours
    public float GetDayTimeInHours ()
    {
        return ConvertSecondsToGameHours (dayTimeInGame);
    }
 
    // Converts specified Seconds to Game Seconds
    private float ConvertSecondsToGameSeconds (float seconds)
    {
        return seconds / SECONDS_PER_GAMESECOND;
    }
 
    // Converts specified Seconds to Game Minutes
    private float ConvertSecondsToGameMinutes (float seconds)
    {
        return seconds / GAMESECONDS_PER_GAMEMINUTE;
    }
 
    // Converts specified Seconds to Game Hours
    private float ConvertSecondsToGameHours (float seconds)
    {
        return ConvertSecondsToGameMinutes (seconds) / GAMEMINUTES_PER_GAMEHOUR;
    }
}
