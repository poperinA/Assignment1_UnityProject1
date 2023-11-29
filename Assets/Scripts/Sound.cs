using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{

    public List<AudioClip> concreteFS;
    public List<AudioClip> dirtFS;
    public List<AudioClip> metalFS;
    public List<AudioClip> sandFS;
    public List<AudioClip> woodFS;

    enum FSMaterial
    {
        Concrete,Dirt,Metal,Sand,Wood,Empty
    }

    private AudioSource footstepSource;

    // Start is called before the first frame update
    void Start()
    {
        footstepSource = GetComponent<AudioSource>();
    }

    private FSMaterial SurfaceSelect()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);
        Material surfaceMaterial;

        if(Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Renderer surfaceRenderer = hit.collider.GetComponentInChildren<Renderer>();
            if (surfaceRenderer)
            {
                surfaceMaterial = surfaceRenderer ? surfaceRenderer.sharedMaterial : null;
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
        return FSMaterial.Empty;
    }

    // Update is called once per frame
    void playFootstep()
    {
        AudioClip clip = null;

        FSMaterial surface = SurfaceSelect();

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


        if (clip != null)
        {
            footstepSource.volume = Random.Range(0.04f, 0.07f);
            footstepSource.pitch = Random.Range(0.5f, 1.2f);
            footstepSource.PlayOneShot(clip);
        }
    }
}
