using UnityEngine;
using System.Collections;
using System;

/**
 * Handles generic plant logic for growing, withering, watering,
 * and killing plants.
 */
public class Plant : MonoBehaviour
{
    const int MIN_LIFE = 0;
    int nightsOld;
    int nightsSinceGrowth;
    int curLife;
    bool canBeWatered;
    PlantState plantState;
    PlantState initialState = PlantState.Seed;
    GameObject waterDrop;

    // Public Attributes
    public int itemGrownID;
    public int nightsPerGrowth;
    public int maxLife;

    // Materials
    public Material seedMat;
    public Material sproutMat;
    public Material adultMat;
    public Material witheredMat;

    public enum PlantState
    {
        Seed = 0,
        Sprout,
        Adult,
        Withered
    }

    void Awake()
    {
        Transform lastChild = transform;
        foreach ( Transform child in transform )
        {
            lastChild = child;
        }
        waterDrop = lastChild.gameObject;
    }

    void Start ()
    {
        if (nightsPerGrowth == 0) {
            Debug.LogError ("nightsPerGrowth is 0 which is invalid. Setting to 1.");
            nightsPerGrowth = 1;
        }
        if (maxLife == 0) {
            Debug.LogError ("maxLife is 0 which is invalid. Setting to 1.");
            maxLife = 1;
        }
        nightsOld = 0;
        nightsSinceGrowth = 0;

        // Set HP to one less than full so watering can make a difference on first day.
        curLife = maxLife - 1;
        if (curLife == MIN_LIFE) {
            SetDry (true);
        }
        SetPlantState (initialState);

        // Initialize the text
        TextMesh textMesh = (TextMesh)GetComponentInChildren<TextMesh> ();
        textMesh.text = (this.name.Substring (0, 1));
    }

    /*
     * Perform our nightly aging of the plant. All logic for withering,
     * growing, and aging the plant should go here.
     */
    public void NightlyUpdate ()
    {
        nightsOld++;
        nightsSinceGrowth++;
        curLife--;

        if (plantState != PlantState.Withered) {
            // Check for wither
            if (curLife < MIN_LIFE) {
                SetPlantState (PlantState.Withered);
                return;
            }
            // Check if the plant needs water.
            bool isDry = canBeWatered && curLife == MIN_LIFE;
            SetDry (isDry);

            // Try to grow
            if (nightsSinceGrowth >= nightsPerGrowth) {
                Grow ();
            }
        }
    }

    /*
     * Put the plant in it's next stage and reset the days since last growth.
     */
    private void Grow ()
    {
        // Withered and Adult plants can't grow
        PlantState nextState;
        if (plantState == PlantState.Adult || plantState == PlantState.Withered) {
            return;
        } else if (plantState == PlantState.Seed) {
            nextState = PlantState.Sprout;
        } else if (plantState == PlantState.Sprout) {
            nextState = PlantState.Adult;
        } else {
            Debug.LogError ("Plant is trying to grow from an unknown state!");
            return;
        }

        // Grow the plant to the next state
        nightsSinceGrowth = 0;
        SetPlantState (nextState);
    }

    /*
     * Set the Plant's state to the specified state and render it.
     */
    private void SetPlantState (PlantState newState)
    {
        plantState = newState;
        if (newState == PlantState.Withered || newState == PlantState.Adult) {
            SetDry (false);
            canBeWatered = false;
        }
        else {
            canBeWatered = true;
        }
        RenderPlantState ();
    }

    /*
     * Update the plant state to the appropriate material.
     */
    void RenderPlantState ()
    {
        Material stateMaterial = seedMat;
        if (plantState == PlantState.Seed) {
            stateMaterial = seedMat;
        } else if (plantState == PlantState.Sprout) {
            stateMaterial = sproutMat;
        } else if (plantState == PlantState.Adult) {
            stateMaterial = adultMat;
        } else if (plantState == PlantState.Withered) {
            stateMaterial = witheredMat;
        }
        renderer.material = stateMaterial;
    }

    /*
     * Turn on or off the elements that show a plant needs water
     */
    private void SetDry (bool dry)
    {
        waterDrop.SetActive(dry);
    }

    /*
     * If the plant is already watered, set it as watered and
     * make it visually obvious.
     */
    public void Water ()
    {
        SetDry (false);
        curLife = maxLife;
    }

    /*
     * Return if the plant is withered.
     */
    public bool isWithered ()
    {
        return plantState == PlantState.Withered;
    }

    /*
     * Return if the plant is ready to be picked.
     */
    public bool isRipe ()
    {
        return plantState == PlantState.Adult;
    }

    /*
     * Sets the plant to initialze to the adult state.
     */
    public void StartAsAdult()
    {
        initialState = PlantState.Adult;
    }
}