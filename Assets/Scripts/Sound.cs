using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    //reference to the animator
    public Animator mAnimator;

    //lists of footstep sounds for different materials
    public List<AudioClip> concreteFS;
    public List<AudioClip> dirtFS;
    public List<AudioClip> metalFS;
    public List<AudioClip> sandFS;
    public List<AudioClip> woodFS;

    //enumeration representing different materials
    enum FSMaterial
    {
        Concrete,Dirt,Metal,Sand,Wood,Empty
    }

    //reference to the AudioSource component
    private AudioSource footstepSource;

    void Start()
    {
        //get the AudioSource component attached to the Racer GameObject
        footstepSource = GetComponent<AudioSource>();
    }

    //function to check the surface material under the player
    private FSMaterial SurfaceSelect()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
        Material surfaceMaterial;

        //perform a raycast to detect the material under the player
        if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
            if (surfaceRenderer)
            {
                surfaceMaterial = surfaceRenderer ? surfaceRenderer.sharedMaterial : null;

                //check the name of the material and return the corresponding FSMaterial
                if (surfaceMaterial.name.Contains("Concrete"))
                {
                    return FSMaterial.Concrete;
                }
                if (surfaceMaterial.name.Contains("Dirt"))
                {
                    return FSMaterial.Dirt;
                }
                if (surfaceMaterial.name.Contains("Metal"))
                {
                    return FSMaterial.Metal;
                }
                if (surfaceMaterial.name.Contains("Sand"))
                {
                    return FSMaterial.Sand;
                }
                if (surfaceMaterial.name.Contains("Wood"))
                {
                    return FSMaterial.Wood;
                }
                else
                {
                    return FSMaterial.Empty;
                }
            }
        }

        //if no material is detected, return Empty
        return FSMaterial.Empty;
    }

    //function to play footstep sounds based on the detected surface material
    void playFootstep()
    {
        AudioClip clip = null;

        //determine the surface material under the player
        FSMaterial surface = SurfaceSelect();

        //select and randomize (from list) the appropriate footstep sound based on the surface material
        switch (surface)
        {
            case FSMaterial.Concrete:
                clip = concreteFS[Random.Range(0, concreteFS.Count)];
                break;

            case FSMaterial.Dirt:
                clip = dirtFS[Random.Range(0, dirtFS.Count)];
                break;

            case FSMaterial.Metal:
                clip = metalFS[Random.Range(0, metalFS.Count)];
                break;

            case FSMaterial.Sand:
                clip = sandFS[Random.Range(0, sandFS.Count)];
                break;

            case FSMaterial.Wood:
                clip = woodFS[Random.Range(0, woodFS.Count)];
                break;

            default:
                break;
        }

        Debug.Log(surface);

        //randomizes the sound and pitch by range, and plays the actual sound
        if (clip != null)
        {
            //if the character is running, pitch range is higher than the other animations
            if (mAnimator.GetFloat("PosZ") == 1) //running animation is the only animation with PosZ == 1, so it is referenced this way
            {
                footstepSource.volume = Random.Range(0.08f, 0.2f);
                footstepSource.pitch = Random.Range(1f, 1.6f);
                footstepSource.PlayOneShot(clip);
            }
            //else, any other animation is randomized
            else
            {
                footstepSource.volume = Random.Range(0.08f, 0.2f);
                footstepSource.pitch = Random.Range(0.4f, 1f);
                footstepSource.PlayOneShot(clip);
            }
        }
    }

}
