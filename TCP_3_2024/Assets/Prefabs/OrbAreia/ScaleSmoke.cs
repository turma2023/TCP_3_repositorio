using UnityEngine;
using Fusion;

public class ScaleSmoke : NetworkBehaviour
{
    public float scaleDuration = 5.0f;
    public float TimeDuration = 8.0f;

    public Vector3 initialScale;
    public Vector3 targetScale;
    private float elapsedTime = 0.0f;
    private bool isScalingUp = true;
    public NetworkRunner runner;
    [Networked] private TickTimer duracao { get; set; }

    void Start()
    {
        // runner = FindObjectOfType<Spawner>().Runner;
        transform.localScale = initialScale; // Define a escala inicial do objeto
        duracao = TickTimer.None;
    }

    void Update()
    {
        if (duracao.IsRunning && !duracao.Expired(runner)) return;

        if (isScalingUp)
        {
            ScaleUp();
        }
        else
        {
            ScaleDown();
        }
    }

    private void ScaleUp()
    {
        // Incrementa o tempo decorrido
        elapsedTime += Time.deltaTime;

        // Calcula a fração do tempo decorrido em relação à duração da escala
        float t = elapsedTime / scaleDuration;

        // Interpola a escala do objeto entre a escala inicial e a escala final
        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

        // Verifica se atingiu a escala final
        if (t >= 1.0f)
        {
            duracao = TickTimer.CreateFromSeconds(runner, TimeDuration);
            isScalingUp = false;
            elapsedTime = 0.0f;
        }
    }

    private void ScaleDown()
    {
        // Incrementa o tempo decorrido
        elapsedTime += Time.deltaTime;

        // Calcula a fração do tempo decorrido em relação à duração da escala
        float t = elapsedTime / scaleDuration;

        // Interpola a escala do objeto entre a escala final e a escala inicial
        transform.localScale = Vector3.Lerp(targetScale, initialScale, t);

        // Verifica se atingiu a escala inicial
        if (t >= 1.0f)
        {
            runner.Despawn(Object);
        }
    }
}
