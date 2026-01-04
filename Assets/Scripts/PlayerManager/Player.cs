using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject eyes;
    [HideInInspector]
    public int playerTapCount;
    GameObject explosionParticle, projectTile, spwanedProjectile;
    public enum PLAYERCOLOR
    {
        Yellow,
        Blue,
        Purple
    }
    // Start is called before the first frame update
    void Start()
    {
        explosionParticle = GameManager.instance.explosion;
        projectTile = GameManager.instance.projectileObj;
    }
    private void OnEnable()
    {
        OnScale(1.2f);
        InheritEyes();
        //SET THE PLAYER TAP VALUE
        if (this.tag == "blue")
            playerTapCount = 2;
        else if (this.tag == "yellow")
            playerTapCount = 3;
        else if (this.tag == "purple")
            playerTapCount = 1;
    }

    private void InheritEyes()
    {
        // INSTANTIATE EYES AND MAKE THE ANIMATION
        if (transform.childCount == 0)
        {
            GameObject eyeObj = Instantiate(eyes, transform.position, Quaternion.identity);
            eyeObj.transform.parent = transform;
            eyeObj.transform.GetChild(0).transform.DOScale(1.35f, 1.0f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            eyeObj.transform.GetChild(1).transform.DOScale(1.25f, 1.2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnScale(float newScaleValue)
    {
        transform.DOScale(newScaleValue, 1.0f)
            .SetEase(Ease.InOutSine)
                     .SetLoops(-1, LoopType.Yoyo);
    }
    // Update is called once per frame
    void Update()
    {
        // CHECK IF THERE ANY PROJECTILE OBJECT LEFT IN SCENE OR NOT
        if (GameManager.instance.numOfTapInLevel <= 0)
        {
            if (GameManager.instance.totalPlayerInLevel != 0 && GameObject.FindGameObjectWithTag("projectile") == null)
                GameManager.instance.OnLevelFailed();
        }
    }
    public void PopPlayer()
    {//POP THE PLAYER AND DO THE STUFF
        this.playerTapCount -= 1;
        SoundManager.instance.Play("Pop");
        if (this.playerTapCount == 0)
        {
            GameObject exp = Instantiate(explosionParticle, this.transform.position, Quaternion.identity);
            spwanedProjectile = Instantiate(projectTile, this.transform.position, Quaternion.identity);
            Destroy(exp, 1f);
            GameManager.instance.totalPlayerInLevel -= 1;
            Destroy(this.gameObject);
            Destroy(spwanedProjectile, 3f);
        }
    }
    public void CheckPlayerState(PLAYERCOLOR color)
    {//CHANGE PLAYER STATE WHEN IT POPS
        PopPlayer();
        switch (color)
        {
            case PLAYERCOLOR.Yellow:
                GameManager.instance.GetComponent<CommandManager>().ExecuteCommand(new InstantiateCommand(PLAYERCOLOR.Yellow, PLAYERCOLOR.Blue, transform.position, GameManager.instance.GetComponent<GridLevel>().purplePrefab, this.gameObject, spwanedProjectile));
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.blue;
                tag = "blue";
                break;
            case PLAYERCOLOR.Blue:
                GameManager.instance.GetComponent<CommandManager>().ExecuteCommand(new InstantiateCommand(PLAYERCOLOR.Blue, PLAYERCOLOR.Purple, transform.position, GameManager.instance.GetComponent<GridLevel>().purplePrefab, this.gameObject, spwanedProjectile));
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.purple;
                tag = "purple";
                break;
            case PLAYERCOLOR.Purple:
                GameManager.instance.GetComponent<CommandManager>().ExecuteCommand(new InstantiateCommand(PLAYERCOLOR.Purple, PLAYERCOLOR.Purple, transform.position, GameManager.instance.GetComponent<GridLevel>().purplePrefab, this.gameObject, spwanedProjectile));
                break;
            default:
                Debug.Log("Invalid color");
                break;
        }
        if (GameManager.instance.numOfTapInLevel >= 0)
        {//CHECK IF ALL THE PLAYERS ARE POPED OR NOT IF POPED DECLARED LEVEL COMPLETE 
            if (GameManager.instance.totalPlayerInLevel == 0)
                GameManager.instance.OnLevelComplete();
        }
    }
}
