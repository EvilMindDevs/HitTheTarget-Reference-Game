using UnityEngine;
using HmsPlugin;
using HuaweiMobileServices.Game;
using System.Text;

public class ManagerOfSaveGame : MonoBehaviour
{
    // Start is called before the first frame update
    int maxThumbnailSize;
    int detailSize;
    GameStarterScript gameStarterScript;

    void Start()
    {
        gameStarterScript = GameObject.Find("PauseButton").GetComponent<GameStarterScript>();
        //HMSSaveGameManager.Instance.GetArchivesClient().LimitThumbnailSize.AddOnSuccessListener((x) => { });
        HMSSaveGameManager.Instance.GetArchivesClient().LimitThumbnailSize.AddOnSuccessListener(LimitThumbnailSizeSuccess);
        HMSSaveGameManager.Instance.GetArchivesClient().LimitDetailsSize.AddOnSuccessListener(LimitDetailSizeSuccess);

        HMSSaveGameManager.Instance.SelectedAction = SelectedActionCreator;
        HMSSaveGameManager.Instance.AddAction = AddActionCreator;
    }

    private void LimitDetailSizeSuccess(int thumbnailSize)
    {
        maxThumbnailSize = thumbnailSize;
    }

    private void LimitThumbnailSizeSuccess(int returnedDetailSize)
    {
        detailSize = returnedDetailSize;
    }

    private void SelectedActionCreator(ArchiveSummary archiveSummary)
    {
        //load your game
        Debug.Log("YOU ENTERED SELECTED ACTION CALLBACK!");
        long score = archiveSummary.CurrentProgress;
        long rockCount = archiveSummary.ActiveTime;


        if (GameManager.rockCount <= 0)
        {
            gameStarterScript.PlayGameWithParameters((int)score, (int)rockCount);
        }
        else
        {
            Debug.Log("Cannot load a finished game");
        }
        
        //start the game but change the parameters to load.
    }

    private void AddActionCreator(bool obj)
    {
        if(GameManager.rockCount != 0)
        {
            //save your game
            string description = "Rock:" + GameManager.rockCount + " Score:" + GameManager.score;
            long playedTime = GameManager.rockCount; //rock count
            long progress = GameManager.score;
            ArchiveDetails archiveContents = new ArchiveDetails.Builder().Build();
            archiveContents.Set(Encoding.ASCII.GetBytes(progress + description + playedTime));
            ArchiveSummaryUpdate archiveSummaryUpdate =
            new ArchiveSummaryUpdate.Builder()
                    .SetActiveTime(playedTime)
                    .SetCurrentProgress(progress)
                    .SetDescInfo(description)
                    //.SetThumbnail(bitmap)
                    //.SetThumbnailMimeType(imageType)
                    .Build();
            HMSSaveGameManager.Instance.GetArchivesClient().AddArchive(archiveContents, archiveSummaryUpdate, true).AddOnSuccessListener((archiveSummary) => {
                string fileName = archiveSummary.FileName;
                string archiveId = archiveSummary.Id;
                //if you wanna use these you can. But this just indicates that it is successfully saved.
                print("fileName is: " + fileName + " and archiveId is " + archiveId);
                print("GamePlayer is: " + archiveSummary.GamePlayer + " and GameSummary is " + archiveSummary.GameSummary);
                print("CurrentProgress is: " + archiveSummary.CurrentProgress + " and ActiveTime is " + archiveSummary.ActiveTime);
            }).AddOnFailureListener((exception) => {
                print("statusCode:" + exception.GetBaseException());
            });
        }
        else
        {
            print("Game is over. Cannot save a finished game!");
        }
    }

}