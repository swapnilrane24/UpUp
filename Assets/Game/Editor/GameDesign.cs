using UnityEngine;
using UnityEditor;
using System;

using GL = UnityEngine.GUILayout;
using EGL = UnityEditor.EditorGUILayout;

[System.Serializable]
public class GameDesign : EditorWindow
{
    //Editor (3 sections Audio , MainMenu , Admob)
    Texture2D book;
    Texture2D GDbanner;
    bool[] toggles;
    string[] buttons;
    private static Texture2D bgColor;
    public GUISkin editorSkin;
    Vector2 scrollPosition = new Vector2(0, 0);
    string[] bannerPositionTexts = new string[] { "Bottom", "Bottom Left", "Bottom Right", "Top", "Top Left", "Top Right" };
    public ManageVariables vars;
    public string listType;
    public GameObject gameUI;

    [MenuItem("Tools/Game Design")]
    static void Initialize()
    {
        GameDesign window = EditorWindow.GetWindow<GameDesign>(true, "GAME DESIGN");
        window.maxSize = new Vector2(550f, 635f);
        window.minSize = window.maxSize;
    }

    void OnEnable()
    {
        toggles = new bool[] { false };
        buttons = new string[] { "Open" };
        vars = (ManageVariables)AssetDatabase.LoadAssetAtPath("Assets/Game/Resources/ManageVariablesContainer.asset", typeof(ManageVariables));
        book = Resources.Load("question", typeof(Texture2D)) as Texture2D;
        GDbanner = Resources.Load("GDbanner", typeof(Texture2D)) as Texture2D;
    }

    void OnGUI()
    {
        // Settings
        bgColor = (Texture2D)Resources.Load("editorBgColor");
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), bgColor, ScaleMode.StretchToFill);
        GUI.skin = editorSkin;
        GL.Label(GDbanner);
        scrollPosition = GL.BeginScrollView(scrollPosition);

        #region Other Options
        // Start Block
        blockHeader("Other Options", "AdMob, Google Play Services and etc. options.", 0);
        if (toggles[0])
        {
            buttons[0] = "Close";
            GL.BeginVertical("GroupBox");

            //Admob
            if (GUILayout.Button("Download Admob SDK"))
            {
                Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases");
            }
            GL.Label("AdMob Options", EditorStyles.boldLabel);
            vars.admobActive = EGL.Toggle("Use Admob Ads", vars.admobActive, "Toggle");
            if (vars.admobActive)
            {
                AssetDefineManager.AddCompileDefine("AdmobDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS });

                //Admob App ID
                vars.admobAppID = EGL.TextField("AdMob App ID", vars.admobAppID);
                separator();
                //Banner
                vars.adMobBannerID = EGL.TextField("AdMob Banner ID", vars.adMobBannerID);
                GL.BeginHorizontal();
                GL.Label("Banner Position");
                vars.bannerAdPoisiton = GL.SelectionGrid(vars.bannerAdPoisiton, bannerPositionTexts, 3, "Radio");
                GL.EndHorizontal();
                separator();

                //Interstitial
                vars.adMobInterstitialID = EGL.TextField("AdMob Interstitial ID", vars.adMobInterstitialID);
                GL.BeginHorizontal();
                GL.Label("Show Interstitial After Death Times");
                vars.showInterstitialAfter = EGL.IntSlider(vars.showInterstitialAfter, 1, 25);
                GL.EndHorizontal();
            }
            else if (!vars.admobActive)
            {
                AssetDefineManager.RemoveCompileDefine("AdmobDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS });
            }
            separator();

            //Google Play Service
            if (GUILayout.Button("Download Google Play SDK"))
            {
                Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
            }
            GL.Label("Google Play Or Game Center", EditorStyles.boldLabel);

            vars.leaderBoardID = EGL.TextField("Leaderboard ID", vars.leaderBoardID);
            GL.Space(5f);
            vars.googlePlayActive = EGL.Toggle("Leaderboard For Google", vars.googlePlayActive, "Toggle");

            if (vars.googlePlayActive)
            {
#if UNITY_ANDROID
                AssetDefineManager.AddCompileDefine("GooglePlayDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android });
#endif
            }
            else if (!vars.googlePlayActive)
            {
#if UNITY_ANDROID
                AssetDefineManager.RemoveCompileDefine("GooglePlayDef",
                    new BuildTargetGroup[] { BuildTargetGroup.Android });
#endif
            }

            separator();

            GL.Label("Other Options", EditorStyles.boldLabel);
            //Rate Url
            GL.BeginHorizontal();
            GL.Label("Rate Button Url", GL.Width(100f));
            vars.rateButtonUrl = EGL.TextArea(vars.rateButtonUrl, GL.Height(20f));
            GL.EndHorizontal();
            GL.Space(15f);
            //Facebook Url
            GL.BeginVertical();
            GL.Label("FB Page ID", GL.Width(100f));
            GL.Label("Example:- 275994256098966 , angrybirds", GL.Width(250f));
            vars.facebookBtnUrl = EGL.TextArea(vars.facebookBtnUrl, GL.Height(20f));
            GL.EndVertical();
            GL.Space(15f);
            //Twitter Url
            GL.BeginVertical();
            GL.Label("Twitter URL", GL.Width(100f));
            GL.Label("Example:- https://twitter.com/SwapnilRane9", GL.Width(250f));
            vars.twitterBtnUrl = EGL.TextArea(vars.twitterBtnUrl, GL.Height(20f));
            GL.EndVertical();
            GL.Space(15f);
            separator();

            EditorUtility.SetDirty(vars);
            GL.EndVertical();
        }
        else buttons[0] = "Open";
        GL.EndVertical();
        // End Block
        #endregion

        GL.EndScrollView();
        EditorUtility.SetDirty(vars);

    }

    void OnDestroy()
    {
        EditorUtility.SetDirty(vars);
    }

    void blockHeader(string mainHeader, string secondHeader, int blockIdex)
    {
        BV();
        GL.Label(mainHeader, "TL Selection H2");
        BH();
        if (GL.Button(buttons[blockIdex], GL.Height(25f), GL.Width(50f))) toggles[blockIdex] = !toggles[blockIdex];
        BHS("HelpBox");
        GL.Label(secondHeader, "infoHelpBoxText");
        GL.Label(book, GL.Height(18f), GL.Width(20f));
        EH();
        EH();
        GL.Space(3);
    }

    void separator()
    {
        GL.Space(10f);
        GL.Label("", "separator", GL.Height(1f));
        GL.Space(10f);
    }

    void BH()
    {
        GL.BeginHorizontal();
    }

    void BHS(string style)
    {
        GL.BeginHorizontal(style);
    }

    void EH()
    {
        GL.EndHorizontal();
    }

    void BVS(string style)
    {
        GL.BeginVertical(style);
    }

    void BV()
    {
        GL.BeginVertical();
    }

    void EV()
    {
        GL.EndVertical();
    }

}