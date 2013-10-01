using UnityEngine;
using System.Collections;
using System;

/**
 * Handles generic plant logic for growing, withering, watering,
 * and killing plants.
 */
public class Plant : MonoBehaviour
{
    int nightsOld;
    int nightsSinceGrowth;
    PlantStates plantState;
    const int MIN_LIFE = 0;
    int curLife;
    bool canBeWatered;

    // Public Attributes
    public int nightsPerGrowth;
    public int maxLife;

    // Materials
    public Material seedMat;
    public Material sproutMat;
    public Material adultMat;
    public Material witheredMat;

    public enum PlantStates
    {
        Seed = 0,
        Sprout,
        Adult,
        Withered
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
        SetPlantState (PlantStates.Seed);

        // Initialize the text
        TextMesh textMesh = (TextMesh)GetComponentInChildren<TextMesh> ();
        textMesh.text = (this.name.Substring (0, 1));
        textMesh.characterSize = 0.7f;
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

        if (plantState != PlantStates.Withered) {
            // Check for wither
            if (curLife < MIN_LIFE) {
                SetPlantState (PlantStates.Withered);
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
        PlantStates nextState;
        if (plantState == PlantStates.Adult || plantState == PlantStates.Withered) {
            return;
        } else if (plantState == PlantStates.Seed) {
            nextState = PlantStates.Sprout;
        } else if (plantState == PlantStates.Sprout) {
            nextState = PlantStates.Adult;
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
    private void SetPlantState (PlantStates newState)
    {
        plantState = newState;
        if (newState == PlantStates.Withered || newState == PlantStates.Adult) {
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
        if (plantState == PlantStates.Seed) {
            stateMaterial = seedMat;
        } else if (plantState == PlantStates.Sprout) {
            stateMaterial = sproutMat;
        } else if (plantState == PlantStates.Adult) {
            stateMaterial = adultMat;
        } else if (plantState == PlantStates.Withered) {
            stateMaterial = witheredMat;
        }
        renderer.material = stateMaterial;
    }

    /*
     * Turn on or off the elements that show a plant needs water
     */
    private void SetDry (bool dry)
    {
        light.enabled = dry;
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
        return plantState == PlantStates.Withered;
    }

    /*
     * Return if the plant is ready to be picked.
     */
    public bool isRipe ()
    {
        return plantState == PlantStates.Adult;
    }

}