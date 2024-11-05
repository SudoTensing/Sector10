using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicAudio : MonoBehaviour {


    public enum MaterialType {
        wood,
        glass,
        metal,
        stone,
        body,
        meat,
        cardboard
    }

    SoundLibrary LIB;
    public MaterialType propType = MaterialType.wood;
    private Rigidbody propBody;
    private AudioSource Audio;

    public bool canPocket = false;

    [Range(0.1f, 10f)]
    public float hitDetection = 0f;

    [Range(0f, 10000f)]
    public float Health = 100f;

    public bool CanDie = true;
    private float dieTime = 0f;
    private bool isDead = false;
    private UniStormCharacterController player;

    void Start() {
        LIB = GameObject.Find("SoundLib").gameObject.GetComponent<SoundLibrary>();
        propBody = this.GetComponent<Rigidbody>();
        Audio = this.GetComponent<AudioSource>();
        player = this.GetComponent<UniStormCharacterController>();
    }


    public void makePlayerRagdoll() {
        int rng2 = Random.Range(1, 4);
        Audio.clip = LIB.returnAudio(propType + "_impact_" + rng2);
        Audio.volume = Audio.volume * 1.5f;
        Audio.pitch = Random.Range(0.9f, 1.1f);
        Audio.Play();
        player.isRagdoll = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (propBody.velocity.magnitude > hitDetection) {

            //Debug.Log($"{this.gameObject.name} @ {propBody.velocity.magnitude}");

            // take damage equal to the velocity of the prop at the current time of collision.
            if (CanDie) {
                Health -= propBody.velocity.magnitude;
            }


            if (propType != MaterialType.body) {
                // there are 3 wood impact sounds - pick a random and fetch it.
                int rng = Random.Range(1, 4);
                Audio.clip = LIB.returnAudio(propType + "_impact_" + rng);
                Audio.pitch = Random.Range(0.9f, 1.1f);
                // Play the Audio Sound if not playing already
                if (!Audio.isPlaying) {
                }
                Audio.Play();
            }

        }



    }

    private void FixedUpdate() {

        if (Health <= 0 && !isDead) {
            if (propType != MaterialType.body) {
                int rng2 = Random.Range(1, 6);
                Audio.clip = LIB.returnAudio(propType + "_break_" + rng2);
                Audio.volume = Audio.volume * 1.5f;
                Audio.pitch = Random.Range(0.9f, 1.1f);
                Audio.Play();
                this.GetComponent<MeshRenderer>().enabled = false;
                if (GetComponent<MeshCollider>()) {
                    this.GetComponent<MeshCollider>().enabled = false;
                }
                if (GetComponent<BoxCollider>()) {
                    this.GetComponent<BoxCollider>().enabled = false;
                }
                
                // spawn gibs
                dieTime = Time.time + 1.5f;
                isDead = true;
            }
        }

    }



    public void invokeHit() {
        if (isDead) { return; }
        // there are 3 wood impact sounds - pick a random and fetch it.
        int rng = Random.Range(1, 4);
        Audio.clip = LIB.returnAudio(propType + "_impact_" + rng);
        Audio.pitch = Random.Range(0.9f, 1.1f);
        // Play the Audio Sound if not playing already
        if (!Audio.isPlaying) {
        }
        Audio.Play();
    }

    public void Break() {
        isDead = true;
        dieTime = 1f;
    }

    private void Update() {
        if (Time.time > dieTime && dieTime != 0f) {
            Destroy(this.gameObject);
        }
    }

}
