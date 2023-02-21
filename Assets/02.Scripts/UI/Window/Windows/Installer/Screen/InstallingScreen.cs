using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InstallingScreen : InstallerScreen
{
    [SerializeField]
    private RectTransform installBarFillTrm;


    public override void EnterScreen()
    {
        UseBackButton();
        UseNextButton();
        installer.CancelBtn.interactable = false;
        NextBtn.interactable = false;
        BackBtn.interactable = false;

        installer.UnInteractableCancelBtn();

        StartCoroutine(InstallBarFill());
    }

    private void EndInstall()
    {
        installer.EndInstall();
        NextBtn.interactable = true;
    }
    private IEnumerator InstallBarFill()
    {
        installBarFillTrm.localScale = new Vector3(0, 1, 1);

        while (installBarFillTrm.localScale.x < 1f)
        {
            float fillSpeed = Random.Range(1f, 3f);
            Vector3 newScale = installBarFillTrm.localScale;
            float value = Time.deltaTime * fillSpeed;
            if (newScale.x + value >= 1)
            {
                newScale.x = 1;
            }
            else
            {
                newScale.x += value;
            }

            installBarFillTrm.localScale = newScale;

            yield return new WaitForSeconds(value);
        }

        installBarFillTrm.localScale = Vector3.one;

        EndInstall();
    }
}
