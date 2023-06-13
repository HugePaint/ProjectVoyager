using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChipGainUIManager : MonoBehaviour
{
    public ChipBoxManager chipBoxManager;
    public CanvasGroup shadowCanvasGroup;

    public CanvasGroup backToMainButtonCanvasGroup;
    public int chipViewed;
    public int targetMaxChip;

    public CanvasGroup blackCoverCanvasGroup;
    public GameObject clickCover;

    public List<ChipBox> chipBoxes;
    public List<Transform> positionReferenceTransforms;
    public List<ChipUI> chipUIs;

    public List<GameObject> shredGameObjects;

    private void Awake()
    {
        blackCoverCanvasGroup.gameObject.SetActive(true);
        clickCover.SetActive(true);
        blackCoverCanvasGroup.DOFade(0f, 2f).From(1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            blackCoverCanvasGroup.gameObject.SetActive(false);
        });
        Global.Battle.chipGainUIManager = this;
        chipViewed = 0;
        targetMaxChip = 6;
        backToMainButtonCanvasGroup.DOFade(0, 0);
    }

    private void Start()
    {
        if (Global.Battle.battleData.isGainingChips)
        {
            chipBoxManager.gameObject.SetActive(true);
        }
    }

    public void AdjustChipUINumber(int chipGetNum)
    {
        targetMaxChip = chipGetNum;
        switch (chipGetNum)
        {
            case 1:
                for (var index = 0; index < shredGameObjects.Count; index++)
                {
                    shredGameObjects[index].SetActive(index == 3);
                }
                for (var index = 0; index < chipBoxes.Count; index++)
                {
                    chipBoxes[index].gameObject.SetActive(index == 4);
                }
                chipBoxes[0].id = -1;
                chipBoxes[1].id = -1;
                chipBoxes[2].id = -1;
                chipBoxes[3].id = -1;
                chipBoxes[4].id = 0;
                chipBoxes[5].id = -1;
                break;
            case 2:
                for (var index = 0; index < shredGameObjects.Count; index++)
                {
                    shredGameObjects[index].SetActive(index == 4 || index == 5);
                }
                for (var index = 0; index < chipBoxes.Count; index++)
                {
                    chipBoxes[index].gameObject.SetActive(index == 3 || index == 5);
                }
                chipBoxes[0].id = -1;
                chipBoxes[1].id = -1;
                chipBoxes[2].id = -1;
                chipBoxes[3].id = 0;
                chipBoxes[4].id = -1;
                chipBoxes[5].id = 1;
                break;
            case 3:
                for (var index = 0; index < shredGameObjects.Count; index++)
                {
                    shredGameObjects[index].SetActive(index == 4 || index == 5 || index == 3);
                }
                for (var index = 0; index < chipBoxes.Count; index++)
                {
                    chipBoxes[index].gameObject.SetActive(index == 3 || index == 4 || index == 5);
                }
                chipBoxes[0].id = -1;
                chipBoxes[1].id = -1;
                chipBoxes[2].id = -1;
                chipBoxes[3].id = 0;
                chipBoxes[4].id = 1;
                chipBoxes[5].id = 2;
                break;
            case 4:
                for (var index = 0; index < shredGameObjects.Count; index++)
                {
                    shredGameObjects[index].SetActive(index == 0 || index == 1 || index == 2 || index == 3);
                }
                for (var index = 0; index < chipBoxes.Count; index++)
                {
                    chipBoxes[index].gameObject.SetActive(index == 0 || index == 1 || index == 2 || index == 4);
                }
                chipBoxes[0].id = 0;
                chipBoxes[1].id = 1;
                chipBoxes[2].id = 2;
                chipBoxes[3].id = -1;
                chipBoxes[4].id = 3;
                chipBoxes[5].id = -1;
                break;
            case 5:
                for (var index = 0; index < shredGameObjects.Count; index++)
                {
                    shredGameObjects[index].SetActive(index == 0 || index == 1 || index == 2 || index == 4 || index == 5);
                }
                for (var index = 0; index < chipBoxes.Count; index++)
                {
                    chipBoxes[index].gameObject.SetActive(index == 0 || index == 1 || index == 2 || index == 3 || index == 5);
                }
                chipBoxes[0].id = 0;
                chipBoxes[1].id = 1;
                chipBoxes[2].id = 2;
                chipBoxes[3].id = 3;
                chipBoxes[4].id = -1;
                chipBoxes[5].id = 4;
                break;
            case 6:
                break;
            default:
                break;
        }
        

        foreach (var positionReference in positionReferenceTransforms)
        {
            positionReference.gameObject.SetActive(false);
        }
        
        foreach (var chipUI in chipUIs)
        {
            chipUI.gameObject.SetActive(false);
        }

        for (var i = 0; i < chipGetNum; i++)
        {
            positionReferenceTransforms[i].gameObject.SetActive(true);
            chipUIs[i].gameObject.SetActive(true);
        }

        if (chipGetNum <= 3)
        {
            positionReferenceTransforms[5].parent.gameObject.SetActive(false);
            chipUIs[5].transform.parent.gameObject.SetActive(false);
        }
    }

    public void ViewOneChip()
    {
        chipViewed += 1;
        if (chipViewed >= targetMaxChip)
        {
            backToMainButtonCanvasGroup.DOFade(1,1f).From(0f);
        }
    }

    public void StageThreeAnimation()
    {
        // var spaceX = 50f;
        // DOTween.To(() => spaceX, x => spaceX = x, 200f, 1f).SetEase(Ease.InOutBack);
        //
        // var spaceY = 30f;
        // DOTween.To(() => spaceY, x => spaceY = x, 100f, 1f).SetEase(Ease.InOutBack).OnUpdate(() =>
        // {
        //     gridLayoutGroup.spacing = new Vector2(spaceX,spaceY);
        // });

        for (var index = 0; index < chipBoxes.Count; index++)
        {
            if (chipBoxes[index].id >= 0)
            {
                chipBoxes[index].transform.DOMove(positionReferenceTransforms[chipBoxes[index].id].position, 1f).SetEase(Ease.InOutBack);
            }
        }

        shadowCanvasGroup.DOFade(1, 1f).From(0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            foreach (var chipBox in chipBoxManager.chipBoxes)
            {
                chipBox.Idle();
            }

            Global.DoTweenWait(0.5f, () =>
            {
                clickCover.SetActive(false);
            });
        });
    }
}
