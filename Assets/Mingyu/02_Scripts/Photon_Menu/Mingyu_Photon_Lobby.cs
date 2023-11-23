using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class Mingyu_Photon_Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField]List<PhotonRoomListInfoSync> roomItems;
    int roomCount = 0;
    public GameObject roomListView;
    public GameObject roomListItem;
    public Animation simpleAnim;

    // 민규가 코딩한 부분
    #region
    private PhotonView pv;
    private static Mingyu_Photon_Lobby instance;

    // 채팅 관련 변수
    private ScrollRect  scRect;
    private Text        chat_Text;
    private string      ChatMessage = "";

    // 타이틀
    private GameObject title_UI;

    // 게임 패널 + 로비
    private GameObject lobby;
    private GameObject makeRoom_Panel;
    private GameObject pw_Panel;

    // 토글
    private Toggle pw_Toggle;
    public bool is_OnPw;
    public string roomName;
    #endregion

    public static Mingyu_Photon_Lobby Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType(typeof(Mingyu_Photon_Lobby)) as Mingyu_Photon_Lobby;

                if (instance == null)
                    Debug.Log("No SingleTon OBJ");
            }

            return instance;
        }
    }

    private void Awake()
    {
        PhotonNetwork.GameVersion = "FierceFight 1.0";
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("마스터 서버에 접속중입니다.");
    }

    private void Start()
    {
        // TitleUI 참조 (비활성화 목적)
        if (GameObject.Find("TitleUI") != null)
        {
            title_UI = GameObject.Find("TitleUI");
            Debug.Log("타이틀 참조" + title_UI.name);
        }

        // 채팅창 참조 (Scroll Rect)
        if (GameObject.Find("Chat_Box_ScrollVersion") != null)
        {
            scRect = GameObject.Find("Chat_Box_ScrollVersion").GetComponent<ScrollRect>();
            Debug.Log("채팅창 참조" + scRect.name);
        }

        // 채팅 참조 (text)
        if (GameObject.Find("Chat_Text") != null)
        {
            chat_Text = GameObject.Find("Chat_Text").GetComponent<Text>();
            Debug.Log("채팅(text) 참조" + chat_Text.name);
        }

        //  방 만들기 패널 참조 (GameObject)
        if (GameObject.Find("MakeRoomPanel") != null)
        {
            makeRoom_Panel = GameObject.Find("MakeRoomPanel");
            Debug.Log("방 패널 참조" + makeRoom_Panel.name);

            makeRoom_Panel.SetActive(false);
        }

        //  패스워드 패널 참조 (GameObject)
        if (GameObject.Find("Pw_Panel") != null)
        {
            pw_Panel = GameObject.Find("Pw_Panel");
            Debug.Log("방 비번 참조" + pw_Panel.name);

            pw_Panel.SetActive(false);
        }

        // 패스워드 토글 참조 (Toggle)
        if (GameObject.Find("Toggle") != null)
        {
            pw_Panel = GameObject.Find("Toggle");
            Debug.Log("패스워드 토글 참조" + pw_Panel.name);
        }

        // 로비 참조 (GameObject, 비활성화 목적) <이 코드는 start 함수 맨 아래 있어야함>
        if (GameObject.Find("Lobby") != null)
        {
            lobby = GameObject.Find("Lobby");
            lobby.SetActive(false);
        }
    }

    #region 방 리스트를 업데이트 하는 부분 (호현이쪽 코드)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo item in roomList) 
        {
            if (!item.RemovedFromList)
            {
                Transform earlyRoom = roomListView.transform.Find(item.Name);
                RectTransform roomTemp;
                if (earlyRoom != null)
                {
                    roomTemp = earlyRoom.GetComponent<RectTransform>();
                }
                else
                {
                    roomTemp = GameObject.Instantiate(roomListItem).GetComponent<RectTransform>();
                    roomTemp.SetParent(roomListView.transform);
                    roomTemp.localScale = new Vector3(1, 1, 1);
                }
                PhotonRoomListInfoSync sync = roomTemp.GetComponent<PhotonRoomListInfoSync>();
                if (sync)
                {
                    sync.name = item.Name;
                    sync.roomNameT.text = item.Name;
                    sync.playerNumberT.text = item.PlayerCount.ToString();
                }
                if (item != null)
                    roomItems.Add(sync);
            }
            else 
            {
                foreach (PhotonRoomListInfoSync room in roomItems) 
                {
                    if (room.name == item.Name)
                    {
                        roomItems.Remove(room);
                        Destroy(room.gameObject);
                        break;
                    }
                }
            }
        }
        roomCount = roomList.Count; Debug.Log(roomList.Count);

    }
    #endregion

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터 서버 접속 성공");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        simpleAnim.Play();
        Debug.Log("로비에 접속하였습니다.");
    }
    public void ObjectSet(GameObject obj, bool value) 
    {
        obj.SetActive(value);
    }

    // 유저가 아이디를 입력하고 들어가면, Lobby라는 방으로 입장한다.
    // -> 방에 들어가야, 채팅이 되기 때문
    public void SetPlayerName(InputField inputName) 
    {
        PhotonNetwork.NickName = inputName.text;

        // Lobby라는 방이 있다면, 들어가고 + 없다면, 생성해서 들어간다.
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, 
            new TypedLobby("Lobby", LobbyType.Default));
    }

    public void CreateRoomBtn() 
    {
        // 테스트 용입니다.
        PhotonNetwork.CreateRoom("TestRoom" + roomCount, new RoomOptions { MaxPlayers = 2 });
    }

    public void GameStart(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    #region 로비 채팅 부분 코딩
    public void Chatting_Lobby(InputField inputChatting)
    {
        ChatMessage = inputChatting.text;
        inputChatting.text = string.Empty;

        pv = this.GetComponent<PhotonView>();
        pv.RPC("ChatInfo", RpcTarget.All, ChatMessage);
    }

    public void ShowChat(string chat)
    {
        chat_Text.text += chat + "\n";
        scRect.verticalNormalizedPosition = 1.0f;
    }

    [PunRPC]
    public void ChatInfo(string sChat)
    {
        ShowChat(sChat);
    }
    #endregion

    #region 버튼 클릭 함수들
    public void BtnEvent_ExitGame(GameObject blackScreen)
    {
        blackScreen.SetActive(true);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 방 만들기 입력
    public void BtnEvent_MakeRoomBtn()
    {
        Debug.Log("방 생성 버튼 클릭");

        title_UI.SetActive(false);
        lobby.SetActive(false);

        makeRoom_Panel.SetActive(true);
    }

    public void BtnEvent_MakeOk()
    {
        makeRoom_Panel.SetActive(false);
        PhotonNetwork.LeaveRoom();

        roomName = makeRoom_Panel.transform.Find("RoomName_InputField").
            GetComponent<InputField>().text;
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();
        StartCoroutine(EnterWaitRoom(roomName));
    }

    IEnumerator EnterWaitRoom(string roomName)
    {
        // 로비에 들어갈 때까지 대기
        yield return new WaitUntil(() => PhotonNetwork.InLobby);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions {MaxPlayers  = 2 });

        // 방에 들어갈 때까지 대기
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    public void BtnEvent_EnterRoom()
    {
        // 클릭한 방이 패스워드가 있다면, 패드워드 창 띄우기
        if (is_OnPw)
        {
            makeRoom_Panel.SetActive(false);
            pw_Panel.SetActive(true);
        }

        // 없다면, 방에 들어가기
        else
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LoadLevel("WaitingRoom");
        }
    }

    // 취소 클릭
    public void BtnEvent_Exit()
    {
        Debug.Log("취소 입력");

        title_UI.SetActive(true);
        lobby.SetActive(true);
        
        pw_Panel.SetActive(false);
        makeRoom_Panel.SetActive(false);
    }

    #endregion
}
