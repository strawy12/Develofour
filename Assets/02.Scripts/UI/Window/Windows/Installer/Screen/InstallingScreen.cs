using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.TerrainTools;
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
        NextBtn.interactable = true;
    }

    private IEnumerator InstallBarFill()
    {
        installBarFillTrm.localScale = new Vector3(0, 1, 1);

        while (installBarFillTrm.localScale.x < 1f)
        {
            float fillSpeed = Random.Range(1f, 10f);
            Vector3 newScale = installBarFillTrm.localScale;
            newScale.x += Time.deltaTime * fillSpeed;
            installBarFillTrm.localScale = newScale;

            yield return new WaitForSeconds(fillSpeed * Time.deltaTime);
        }

        installBarFillTrm.localScale = Vector3.one;
        EndInstall();
    }
}
