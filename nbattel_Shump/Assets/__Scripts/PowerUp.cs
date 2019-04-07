using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameObject lastTriggerGo = null;
    [SerializeField]
    private AudioClip _audioClip;

    public WeaponType type;
    private BoundsCheck _bndCheck;

    public float lifeTime = 4.0f;  //Seconds the powerup exists
    public float fadeTime = 2.0f;  //Seconds it will then fade
    public float birthTime;
    private Renderer powerupRend;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Awake()
    {
        powerupRend = this.GetComponent<Renderer>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _bndCheck = GetComponent<BoundsCheck>();
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        if (u >= 1)
        { 
            Destroy(this.gameObject);
            return;
        }

        if(u > 0)
        {
            Color c = powerupRend.material.color;
            c.a = 1f - u; ;
            powerupRend.material.color = c;
        }
    }

    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        type = wt;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        if (go.tag == "Hero")
        {
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);

            if (this.tag == "Shield_Powerup")
            {
                //Shield PowerUp
                if (Hero.S.shieldsActiveBlue == true)
                {
                    Destroy(this.gameObject);
                    return;
                }
                else if (Hero.S.shieldsActiveYellow == true)
                {
                    Hero.S.shieldsActiveYellow = false;
                    Hero.S._shieldYellow.SetActive(false);
                    Hero.S.shieldsActiveBlue = true;
                    Hero.S._shieldBlue.SetActive(true);
                    Hero.S.lives++;
                    _uiManager.UpdateLives(Hero.S.lives);
                }
                else if (Hero.S.shieldsActiveRed == true)
                {
                    Hero.S.shieldsActiveRed = false;
                    Hero.S._shieldRed.SetActive(false);
                    Hero.S.shieldsActiveYellow = true;
                    Hero.S._shieldYellow.SetActive(true);
                    Hero.S.lives++;
                    _uiManager.UpdateLives(Hero.S.lives);
                }
                else
                {
                    Hero.S.shieldsActiveRed = true;
                    Hero.S._shieldRed.SetActive(true);
                    Hero.S.lives++;
                    _uiManager.UpdateLives(Hero.S.lives);
                }

                Destroy(this.gameObject);
            }
            else if (this.tag == "Speed_Powerup")
            {
                Hero.S.SpeedBoostPowerUpOn();
                Destroy(this.gameObject);
            }
            else if (this.tag == "Nuke_Powerup")
            {
                Weapon.nukes++;
                Main.S.nukesLeft++;
                Main.S.updateNukeText();
                Destroy(this.gameObject);
            }

        }
        else
        {
            print("PowerUp hit by non-Hero :" + go.name);
        }                
    }
}
