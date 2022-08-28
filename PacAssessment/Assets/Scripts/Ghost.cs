using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Animator animatorController;
    [SerializeField] private GameObject GhostSpriteGO;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = GhostSpriteGO.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animatorController.SetTrigger("ScaredTrigger");

        if (isDead)
        {
            animatorController.SetTrigger("DeadTrigger");
        }
    }
}
