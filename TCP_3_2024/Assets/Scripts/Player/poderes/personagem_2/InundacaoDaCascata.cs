using System.Collections;
using UnityEngine;

public class InundacaoDaCascata : MonoBehaviour
{
    public float stunDuration = 2f; 
    public float spideeffect_duratin = 2f;

    [Header("Parâmetros de Crescimento")]
    public float growthSpeed = 1f;  
    public float maxSize = 2f;      
    public float minSize = 0.5f;   
    public int pulseCount = 3;      

    private bool isPulsing = false; 

    private void Start()
    {
        StartCoroutine(PulseEffect()); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != gameObject.tag) 
        {
            Debug.Log("Tag diferente! Aplicando stun...");
            PlayerMovement enemy = other.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                //enemy.Stun(stunDuration);
            }
        }
        else 
        {
            Debug.Log("Tag igual! Aplicando speedbuff...");
            PlayerMovement enemy = other.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                //enemy.speedbuff(spideeffect_duratin);
            }
        }
    }

    private IEnumerator PulseEffect()
    {
        isPulsing = true;
        Vector3 originalScale = transform.localScale;

        for (int i = 0; i < pulseCount; i++)
        {
      
            yield return StartCoroutine(ScaleOverTime(originalScale, maxSize, growthSpeed));

       
            yield return StartCoroutine(ScaleOverTime(transform.localScale, minSize, growthSpeed));
        }

  
        gameObject.SetActive(false);
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, float targetScale, float speed)
    {
        Vector3 target = new Vector3(targetScale, targetScale, targetScale);
        float progress = 0f;

        while (progress < 1f)
        {
            
            transform.localScale = Vector3.Lerp(startScale, target, progress);
            progress += Time.deltaTime * speed;
            yield return null;
        }

    
        transform.localScale = target;
    }

    private void Update()
    {
     
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
    }
}
