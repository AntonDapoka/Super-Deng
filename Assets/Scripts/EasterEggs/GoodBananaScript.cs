using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GoodBananaScript : MonoBehaviour
{
    [SerializeField] private FaceGridGeneratorScript FGGS;
    [SerializeField] private int width = 32;
    [SerializeField] private int height = 18;
    [SerializeField] private float frameRate = 15f;
    private GameObject[,] cells; // объекты на сцене
    private FaceScript[,] faceScripts; // объекты на сцене
    public Material blackMaterial;
    public Material whiteMaterial;

    private void Start()
    {
        cells = FGGS.GetFaceGlowingPartGrid();
        faceScripts = FGGS.GetFaceScriptGrid();
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        int frame = 0;

        while (true)
        {
            string path = Path.Combine(Application.streamingAssetsPath, $"{frame:0000}.txt");
            if (!File.Exists(path)) yield break;

            string[] lines = File.ReadAllLines(path);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char pixel = lines[y][x];
                    FaceScript FS = faceScripts[x, y];
                    if (!FS.isLeft &&
                        !FS.isRight &&
                        !FS.isTop &&
                        !FS.havePlayer
                        ) 
                    {
                        cells[x, y].GetComponent<Renderer>().material =
                            pixel == '0' ? whiteMaterial : blackMaterial;

                        Debug.Log(pixel);
                        FS.glowingPart.transform.localScale = new Vector3(1f, 1f, pixel == '0' ? 1 : -((float)pixel) / 10);//
                    }
                    else 
                    {
                        FS.glowingPart.transform.localScale = new Vector3(1f, 1f, 1f);//
                    }

                }
            }

            frame++;
            yield return new WaitForSeconds(1f / frameRate);
        }
    }

    IEnumerator AnimateScale(Transform target, float targetZ, float duration)
    {
        Vector3 startScale = target.localScale;
        Vector3 endScale = new Vector3(startScale.x, startScale.y, targetZ);
        float time = 0f;

        while (time < duration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.localScale = endScale;
    }
}
