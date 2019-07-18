using RemoteAdmin;
using UnityEngine;
using System.Collections.Generic;
using Dissonance;
using System;
using System.Linq;
using UnityEngine.Networking;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dissonance.Integrations.UNet_HLAPI;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;


//using Dissonance.Integrations.UNet_HLAPI;


namespace Cheat
{

    public class Cheat : NetworkBehaviour
    {
        static public Dictionary<string, KeyCode> CheatKeys = new Dictionary<string, KeyCode>
        {
            ["SpeedHack"] = KeyCode.B,
            ["Noclip"] = KeyCode.V,
            ["AntiDoor"] = KeyCode.Z,
            ["SettingsMenu"] = KeyCode.I,
            ["ListenAll"] = KeyCode.L,
            ["Headless"] = KeyCode.H,
            ["SCPMode"] = KeyCode.End,
            ["LocationESP"] = KeyCode.Insert,
            ["PlayerESP"] = KeyCode.Delete,
            ["ItemsESP"] = KeyCode.Home,
            ["WeaponsPro"] = KeyCode.F1,
            ["Electrician"] = KeyCode.F2,
            ["Eject"] = KeyCode.F10,
            ["OpenAllDoors"] = KeyCode.F8,
            ["MicroAim"] = KeyCode.U,
            ["GlobalModWarning"] = KeyCode.X,
            ["AmmoMagnet"] = KeyCode.O,
            ["Stomp"] = KeyCode.G,
            ["Aimbot"] = KeyCode.F,
            ["Freecam"] = KeyCode.K,
            ["CrashServer"] = KeyCode.F4,
            ["Ballerina"] = KeyCode.PageUp,
            ["ChangeAttachments"] = KeyCode.Y
        };
        static public Dictionary<string, float> Settings = new Dictionary<string, float>
        {
            ["AimbotSettings:"] = 777777777777777777777777777f,
            ["AimbotCalls"] = 1f,
            ["AimbotPenetration"] = 1f, // 0 off 1 on
            ["AimbotAngleRange"] = 12f, //angle
            ["AimbotDamageMultiplier"] = 4f, // tenthx, 1x, 2x, 4x
            ["AimbotCoordinateShift"] = 5.5f,
            ["AimbotFriendlyFire"] = 0f,



            ["ESP Settings:"] = 777777777777777777777777777f,
            ["ItemESPRange"] = 200f,
            ["ItemsESPShow"] = -1f, // itemid, -1 showall
            ["ItemRefreshRate"] = 30f,
            ["LocationRefreshRate"] = 80f,

            ["SpeedHack Settings:"] = 777777777777777777777777777f,
            ["RunSpeed"] = 1.19f,
            ["WalkSpeed"] = 1.27f,
            ["JumpHeight"] = 1.3f,
            ["FreecamSpeed"] = 8f,
            ["NoclipSpeed"] = 3.87f,

            

            ["WeaponsPro Settings:"] = 777777777777777777777777777f,
            ["BulletSpread"] = 0f,
            ["FireRate"] = 25f, //all the recoils
            ["FovKick"] = 0f,
            ["BackSpeed"] = 0f,
            ["LerpSpeed"] = 0f,
            ["ShockSize"] = 0f,
            ["UpSize"] = 0f,
            ["reloadswapaway"] = 0.33f,
            ["reloadswapto"] = 0.33f,

            ["Apperance:"] = 777777777777777777777777777f,
            ["TextSize"] = 15,
            ["ShowMenu"] = 1f,
            ["ShowCurAmmo"] = 1f,




            ["Weapon Atachments:"] = 777777777777777777777777777f,
            ["itemInventoryIndex"] = 0f,
            ["itemId"] = 0f,
            ["sight"] = 0f,
            ["barrel"] = 0f,
            ["other"] = 0f,

            ["Other Cheat Settings:"] = 777777777777777777777777777f,
            ["AntiDoorRange"] = 6f,
            ["CrashServerCalls"] = 200f,
            ["BallerinaSpeed"] = 20f,
            ["SCPAutoAttack"] = 1f,
            ["BallerinaDanceMode"] = 0f,
            ["zapcalls"] = 10f,
            ["txtLoadTime"] = 4f,
            ["MicroAimCalls"] = 1f,
            ["StompCalls"] = 5f,


        };
        static public Dictionary<string, bool> CheatsOn = new Dictionary<string, bool>();
        static public Dictionary<string, Action> Cheats = new Dictionary<string, Action>
        {
            ["SpeedHack"] = SpeedHack,
            ["Noclip"] = Noclip,
            ["AntiDoor"] = AntiDoor,
            ["SettingsMenu"] = SettingsMenu,
            ["ListenAll"] = ListenAll,
            ["Headless"] = Headless,
            ["SCPMode"] = SCPMode,
            ["LocationESP"] = LocationESP, 
            ["PlayerESP"] = PlayerESP,
            ["ItemsESP"] = ItemsESP,
            ["WeaponsPro"] = WeaponsPro, 
            ["Electrician"] = Electrician,
            ["Eject"] = Eject,
            ["OpenAllDoors"] = OpenAllDoors,
            ["MicroAim"] = MicroAim,
            ["GlobalModWarning"] = GlobalModWarning,
            ["AmmoMagnet"] = AmmoMagnet,
            ["Stomp"] = Stomp,
            ["Aimbot"] = Aimbot,
            ["Freecam"] = Freecam,
            ["CrashServer"] = CrashServer,
            ["Ballerina"] = Ballerina,
            ["ChangeAttachments"] = ChangeAttachments
        };
       
        private static void ChangeAttachments()
        {
            if (CheatsOn["ChangeAttachments"])
            {
                int i = 0;
                foreach (var item in cInventory.items)
                {
                    if (item.id == cInventory.curItem)
                    {
                        cInventory.DropItem(i, (int)Settings["sight"], (int)Settings["barrel"], (int)Settings["other"]);  
                    }
                    i++;
                }

                CheatsOn["ChangeAttachments"] = false;
            }

        }

        static private PlyMovementSync cPlyMovementSync;
        static private CharacterClassManager cCharacterClassManager;
        static private Inventory cInventory;
        static private PlayerInteract cPlayerInteract;
        static private Searching cSearching;
        static private MicroHID_GFX cMicroHID_GFX;
        static private FootstepSync cFootstepSync;
        static private WeaponManager cWeaponManager;
        static private FirstPersonController cFirstPersonController;
        static private CharacterController cCharacterController;
        static private Scp173PlayerScript cScp173PlayerScript;
        static private Scp049PlayerScript cScp049PlayerScript;
        static private Scp049_2PlayerScript cScp049_2PlayerScript;
        static private Scp939PlayerScript cScp939PlayerScript;
        static private Scp096PlayerScript cScp096PlayerScript;
        static private Scp106PlayerScript cScp106PlayerScript;
        static private Scp079PlayerScript cScp079PlayerScript;
        static private AmmoBox cAmmoBox;
        static private AnimationController cAnimationController;
        static public InventoryDisplay cInventoryDisplay;

        static public void RefreshComponents()
        {
            cPlyMovementSync = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
            cCharacterClassManager = PlayerManager.localPlayer.GetComponent<CharacterClassManager>();
            cInventory = PlayerManager.localPlayer.GetComponent<Inventory>();
            cPlayerInteract = PlayerManager.localPlayer.GetComponent<PlayerInteract>();
            cSearching = PlayerManager.localPlayer.GetComponent<Searching>();
            cMicroHID_GFX = PlayerManager.localPlayer.GetComponent<MicroHID_GFX>();
            cFootstepSync = PlayerManager.localPlayer.GetComponent<FootstepSync>();
            cWeaponManager = PlayerManager.localPlayer.GetComponent<WeaponManager>();
            cFirstPersonController = PlayerManager.localPlayer.GetComponent<FirstPersonController>();
            cCharacterController = PlayerManager.localPlayer.GetComponent<CharacterController>();
            cScp173PlayerScript = PlayerManager.localPlayer.GetComponent<Scp173PlayerScript>();
            cScp049PlayerScript = PlayerManager.localPlayer.GetComponent<Scp049PlayerScript>();
            cScp049_2PlayerScript = PlayerManager.localPlayer.GetComponent<Scp049_2PlayerScript>();
            cScp939PlayerScript = PlayerManager.localPlayer.GetComponent<Scp939PlayerScript>();
            cScp096PlayerScript = PlayerManager.localPlayer.GetComponent<Scp096PlayerScript>();
            cScp106PlayerScript = PlayerManager.localPlayer.GetComponent<Scp106PlayerScript>();
            cScp079PlayerScript = PlayerManager.localPlayer.GetComponent<Scp079PlayerScript>();
            cAmmoBox = PlayerManager.localPlayer.GetComponent<AmmoBox>();
            cAnimationController = PlayerManager.localPlayer.GetComponent<AnimationController>();
            cInventoryDisplay = PlayerManager.localPlayer.GetComponent<InventoryDisplay>();
        }
        static private float LookDirection = 0;
        static private void Ballerina()
        {
            if (CheatsOn["Ballerina"] && !CheatsOn["Headless"])
            {
                cAnimationController.CallCmdSyncData((int)Settings["BallerinaDanceMode"], new Vector2(1, 1));
              //  cFirstPersonController.animationID = (int)Settings["BallerinaDanceMode"];

                 cPlyMovementSync.CallCmdSyncData(LookDirection * Settings["BallerinaSpeed"], PlayerManager.localPlayer.transform.position, Camera.main.transform.rotation.eulerAngles.x);
            //    cPlyMovementSync.GetType().GetField("myRotation", BindingFlags.Instance | BindingFlags.NonPublic).SetValue("myRotation", LookDirection);
                //cPlyMovementSync.m
                LookDirection++;

            }
            else if (CheatsOn["Headless"] && CheatsOn["Ballerina"])
            {
                cAnimationController.CallCmdSyncData((int)Settings["BallerinaDanceMode"], new Vector2(1, 1));
                cPlyMovementSync.CallCmdSyncData(LookDirection * Settings["BallerinaSpeed"], PlayerManager.localPlayer.transform.position, -590);
                LookDirection++;
            }
        }
        static private void Freecam()
        {
            if (CheatsOn["Freecam"] && !CheatsOn["Noclip"])
            {
                cFirstPersonController.noclip = true;
                cCharacterController.enabled = false;
                cPlyMovementSync.enabled = false;
                // Camera.
                // cFirstPersonController.m_UseHeadBob = true;
                Memory.SetSendPacket(false);
                Vector3 MyPos = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    MyPos += Camera.main.transform.forward;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    MyPos -= Camera.main.transform.forward;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    MyPos += Camera.main.transform.right;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    MyPos -= Camera.main.transform.right;
                }

                var speed = Settings["FreecamSpeed"];
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed *= 5;
                }
                PlayerManager.localPlayer.transform.position += MyPos.normalized * speed * Time.deltaTime;
                if (Input.GetKey(KeyCode.Space))
                {
                    PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * speed;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 400f;
                    }
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * speed;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 400f;
                    }
                }
            }
            else if (!CheatsOn["Freecam"])
            {
                Memory.SetSendPacket(true);
                cCharacterController.enabled = true;
                cPlyMovementSync.enabled = true;
                if (!CheatsOn["Noclip"])
                {
                    cFirstPersonController.noclip = false;
                }
                //Camera.main.transform.position = new Vector3(PlayerManager.localPlayer.transform.position.x, PlayerManager.localPlayer.transform.position.y + 1, PlayerManager.localPlayer.transform.position.y);                
              //  cFirstPersonController.m_UseHeadBob = false;
            }
        }
      //  static private Door[] doors = UnityEngine.Object.FindObjectsOfType<Door>();
        static private void OpenAllDoors()
        {
            if (cCharacterClassManager.curClass == -1)
            {
                if (CheatsOn["OpenAllDoors"])
                {
                    foreach (Door door2 in UnityEngine.Object.FindObjectsOfType<Door>())
                    {
                        try
                        {
                            if (!door2.isOpen && (door2.permissionLevel == string.Empty))
                            {                                
                                cPlyMovementSync.CallCmdSyncData(0, new Vector3(door2.transform.position.x, door2.transform.position.y + 1, door2.transform.position.z - 2), 0);
                                cPlayerInteract.CallCmdOpenDoor(door2.gameObject);                             
                            }
                        }
                        catch { }

                    }
                    
                }
                else
                {

                }
            }
        }
        static private string kicker = string.Empty;
        static private void CrashServer()
        {
            if (CheatsOn["CrashServer"])
            {
                for (int i = 0; i < Settings["CrashServerCalls"]; i++)
                {
                    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        cWeaponManager.CallCmdChangeModPreferences("jpanosfdnfdjsa");
                        // cPlyMovementSync.CallCmdSyncData(PlayerManager.localPlayer.transform.rotation.eulerAngles.y, PlayerManager.localPlayer.transform.position, Camera.main.transform.rotation.eulerAngles.x);
                    }
                }
            }
        }

        static private bool AimbotCheck(GameObject target, string hitboxType, Vector3 dir, Vector3 sourcePos, Vector3 targetPos)
        {
            var targetCCM = target.GetComponent<CharacterClassManager>();
            if (Math.Abs(Camera.main.transform.position.y - target.transform.position.y) > 40f) return false;
            if (!(targetCCM != null) || (!cWeaponManager.GetShootPermission(targetCCM, false) && Settings["AimbotFriendlyFire"] != 1f)) return false;
            if ((Settings["AimbotFriendlyFire"] != 1f) && (cCharacterClassManager.klasy[targetCCM.curClass].team == cCharacterClassManager.klasy[cCharacterClassManager.curClass].team)) return false;
            if (Math.Abs(Camera.main.transform.position.y - targetCCM.transform.position.y) > 40f) return false;
            if (Vector3.Distance(Camera.main.transform.position, sourcePos) > 6.5f) return false;
            if (Vector3.Distance(targetCCM.transform.position, targetPos) > 6.5f) return false;
            if (Physics.Linecast(sourcePos, targetPos, cWeaponManager.raycastServerMask)) return false;
            return true;
        }
        static private void Aimbot()
        {
            if (CheatsOn["Aimbot"])
            {
                string damage = string.Empty;
                switch (Settings["AimbotDamageMultiplier"])
                {
                    case 4f:
                        damage = "HEAD";
                        break;
                    case 0.5f:
                        damage = "LEG";
                        break;
                    case 0.1f:
                        damage = "SCP106";
                        break;
                }
                foreach (GameObject enemy in PlayerManager.singleton.players)
                {
                    if ((enemy != PlayerManager.localPlayer) && (Vector3.Angle(Camera.main.transform.forward, enemy.transform.position - Camera.main.transform.position) < Settings["AimbotAngleRange"]))
                    {
                     //   Camera.main.transform.LookAt(enemy.transform.position);
                        var mypos = Camera.main.transform.position;
                        var enemypos = enemy.transform.position;
                        for (int i = 0; i < Settings["AimbotCalls"]; i++)
                        {
                            if (AimbotCheck(enemy, damage, enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position, enemy.transform.position))
                            {
                                cWeaponManager.CallCmdShoot(enemy, damage, enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position, enemy.transform.position);
                            }

                            else if ((Settings["AimbotPenetration"] == 1) && AimbotCheck(enemy, damage, enemy.transform.position - Camera.main.transform.position, new Vector3(mypos.x, mypos.y - Settings["AimbotCoordinateShift"], mypos.z), new Vector3(enemypos.x, enemypos.y - Settings["AimbotCoordinateShift"], enemypos.z)))
                            {

                                cWeaponManager.CallCmdShoot(enemy, damage, enemy.transform.position - Camera.main.transform.position, new Vector3(mypos.x, mypos.y - Settings["AimbotCoordinateShift"], mypos.z), new Vector3(enemypos.x, enemypos.y - Settings["AimbotCoordinateShift"], enemypos.z));

                            }
                        }
                    }
                }
            }
        }
        static private bool _isGlobalMod(string steamid)
        {
            switch (steamid)
            {
                case "76561198056374428": //4rae
                case "76561198139232244": //TheRealBimo
                case "76561198071934271": //zabszk
                case "76561197988477565": //maverick
                case "76561198116439744": //shingeni
                case "76561198067357008": //Takail
                case "76561198828758576": // gravegravegrave
                case "76561198068408917": //KingCobra70
                case "76561198187711341": //mozeman
                case "76561198328956236": //MrRunt
                case "76561198011844757": //jordanlol633
                case "76561198194513838": //peperownik
                case "76561198163284469": //Phoenix--Pronject
                case "76561198137216316": //rahveeohleee
                case "76561198349022866": //rintheweeb
                case "76561198049556261": //RealRomlyn
                case "76561198026973262": //WGSHark
                case "76561198040425824": //ICANHASNUKES
                case "76561198116188982": //AgentBlackout
                case "76561197963326920": //WinterBeyond
                case "76561198035190456": //hlchan
                case "76561198118254410": //androxanik
                case "76561198101074567": //Blizzard098
                case "76561198135154430": //BlueTheKing
                case "76561198078737562": //Dankrushen
                case "76561198113143090": //DjNathann
                case "76561198204575388": //ericthe1234
                case "76561198047770015": //erykol
                case "76561198049611738": //ilysen
                case "76561198019213377": //EvanGames
                case "76561198098290255": //ffian
                case "76561198010095857": //iD4NG3Rs
                case "76561198078443796": //InsaneRed?
                case "76561198093809845": //Keygano
                case "76561198082888468": //klaepek
                case "76561198071607345": //LordOfKhaos
                case "76561198289651857": //joseph_the_electrician
                case "76561198184948159": //MultiverseUncle
                case "76561198041787473": //191123
                case "76561198276465358": //Sinon1
                case "76561198170429887": //TheeRider
                case "76561198059219967": //TheHyde
                case "76561198269333290": //Tr00n11x
                case "76561198074568861": //Voidus
                case "76561198076837733": //wavepoole
                case "76561198219259740": //xEnded_
                case "76561198046639503": //nicku?
                case "76561198071735320": //aHarmlessSpoon?
                case "76561198091199713": //SirMeepington
                case "76561198202123521": //killer_1001
                    return true;
                default:
                    return false;
            }
        }
        static private void GlobalModWarning()
        {
            if (CheatsOn["GlobalModWarning"])
            {
                foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    NicknameSync nickname = player.GetComponent<NicknameSync>();
                    CharacterClassManager targetcCharacterClassManager = player.GetComponent<CharacterClassManager>();
                    if (_isGlobalMod(targetcCharacterClassManager.SteamId))
                    {
                        WarningMessage = "<color=#ff0000><b><size=" + Settings["TextSize"] + ">" + nickname.myNick + " is a global moderator!</size></b></color>";
                    }
                }
            }
        }
        static private Pickup current;
        static private float now = Time.time;
        static private float attempt = -99;
        static private void AmmoMagnet()
        {
            if (CheatsOn["AmmoMagnet"])
            {
                foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
                {
                    if ((item.info.itemId == 28 || item.info.itemId == 29 || item.info.itemId == 22) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, item.transform.position) < 3.5f))
                    {
                        if (current == null || ((Time.time - now) > 1))
                        {
                            cSearching.CallCmdStartPickup(item.gameObject);
                            current = item;
                            now = Time.time;
                        }
                        if (current != null && (Time.time - attempt) > 0.4)
                        {
                            cSearching.CallCmdPickupItem(current.gameObject);
                            attempt = Time.time;
                        }
                    }
                }
            }
        }
        static private void Stomp()
        {
            if(CheatsOn["Stomp"])
            {
                for (int i = 0; i < Settings["StompCalls"]; i++)
                {
                    cFootstepSync.CallCmdSyncFoot(true);
                }
            }
        }

        static private void MicroAim()
        {
            if (CheatsOn["MicroAim"] && (cInventory.curItem == 16))
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                    if ((player != PlayerManager.localPlayer) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 10f))  // < range ???     
                    {
                        for (int i = 0; i < Settings["MicroAimCalls"]; i++)
                        {
                            cMicroHID_GFX.CallCmdHurtPlayersInRange(player);
                        }
                    }
                }
            }
        }
        static private Dictionary<int, Tuple<float, float, float, float, float, float, float>> DefaultWeapons = new Dictionary<int, Tuple<float, float, float, float, float, float, float>>
        {
            [0] = new Tuple<float, float, float, float, float, float, float>(6f, 5f, 1.8f, 1.3f, 1f, 0.12f, 0.13f),
            [1] = new Tuple<float, float, float, float, float, float, float>(10f, 5f, 0.5f, 1.5f, 1f, 0.1f, 0.11f),
            [2] = new Tuple<float, float, float, float, float, float, float>(8f, 5f, 0.2f, 1.5f, 1f, 0.1f, 0.11f),
            [3] = new Tuple<float, float, float, float, float, float, float>(10f, 5f, 0f, 1.5f, 1f, 0.1f, 0.11f),
            [4] = new Tuple<float, float, float, float, float, float, float>(11f, 10f, 0.7f, 1.5f, 1.5f, 0.08f, 0.13f),
            [5] = new Tuple<float, float, float, float, float, float, float>(5f, 10f, 2.5f, 1.5f, 1.5f, 0.15f, 0.15f)
        };

        static private float SwapWait = -99;
        static private float unSwapWait = -99;
        static private int lastitem = -1;
        static private void WeaponsPro()
        {
            if (CheatsOn["WeaponsPro"])
            {
                for (int i = 0; i < cWeaponManager.weapons.Length; i++)
                {
                    cWeaponManager.weapons[i].shotsPerSecond = Settings["FireRate"];
                    cWeaponManager.weapons[i].unfocusedSpread = Settings["BulletSpread"];
                    cWeaponManager.weapons[i].recoil.fovKick = Settings["FovKick"];
                    cWeaponManager.weapons[i].recoil.backSpeed = Settings["BackSpeed"];
                    cWeaponManager.weapons[i].recoil.lerpSpeed = Settings["LerpSpeed"];
                    cWeaponManager.weapons[i].recoil.shockSize = Settings["ShockSize"];
                    cWeaponManager.weapons[i].recoil.upSize = Settings["UpSize"];
                    cWeaponManager.weapons[i].reloadingTime = 0;
                    cWeaponManager.weapons[i].allowFullauto = true;
                }

                if (Input.GetKeyDown(KeyCode.R) && (cAmmoBox.GetAmmo(cWeaponManager.weapons[cWeaponManager.curWeapon].ammoType) != 0))
                {
                    SwapWait = Time.time;

                }


                if ((Time.time - SwapWait < 2) && (Time.time - SwapWait > Settings["reloadswapaway"]))
                {
                    cInventory.CallCmdSyncItem(-1);
                    cInventory.curItem = -1;
                    SwapWait = -99;
                    unSwapWait = Time.time;
                    lastitem = (int)cInventory.GetType().GetField("prevIt", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cInventory);


                }
                if ((Time.time - unSwapWait < 2) && (Time.time - unSwapWait > Settings["reloadswapto"]))
                {
                    if (cInventory.curItem == -1)
                    {
                        cInventory.CallCmdSyncItem(lastitem);
                        cInventory.curItem = lastitem;
                    }
                    unSwapWait = -99;
                }
            }
            else
            {
                unSwapWait = -99;
                SwapWait = -99;
                for (int i = 0; i < cWeaponManager.weapons.Length; i++)
                {
                    cWeaponManager.weapons[i].shotsPerSecond = DefaultWeapons[i].Item1;
                    cWeaponManager.weapons[i].unfocusedSpread = DefaultWeapons[i].Item2;
                    cWeaponManager.weapons[i].recoil.fovKick = DefaultWeapons[i].Item3;
                    cWeaponManager.weapons[i].recoil.backSpeed = DefaultWeapons[i].Item4;
                    cWeaponManager.weapons[i].recoil.lerpSpeed = DefaultWeapons[i].Item5;
                    cWeaponManager.weapons[i].recoil.shockSize = DefaultWeapons[i].Item6;
                    cWeaponManager.weapons[i].recoil.upSize = DefaultWeapons[i].Item7;
                }

            }
        }
        static private void SpeedHack()
        {
            if (CheatsOn["SpeedHack"])
            {
                cFirstPersonController.m_RunSpeed = cCharacterClassManager.klasy[cCharacterClassManager.curClass].runSpeed * Settings["RunSpeed"];
                cFirstPersonController.m_JumpSpeed = cCharacterClassManager.klasy[cCharacterClassManager.curClass].runSpeed * Settings["JumpHeight"];
                cFirstPersonController.m_WalkSpeed = cCharacterClassManager.klasy[cCharacterClassManager.curClass].runSpeed * Settings["WalkSpeed"];
       //         cFirstPersonController.sneaking = true;
            }
            else
            {
                cFirstPersonController.m_RunSpeed = cCharacterClassManager.klasy[cCharacterClassManager.curClass].runSpeed;
                cFirstPersonController.m_JumpSpeed = cCharacterClassManager.klasy[cCharacterClassManager.curClass].jumpSpeed;
                cFirstPersonController.m_WalkSpeed = cCharacterClassManager.klasy[cCharacterClassManager.curClass].walkSpeed;
            }
        }
        static private void DisplayLocation(string name, Vector3 thePosition, Color colour)
        {
            var pos2d = Camera.main.WorldToScreenPoint(thePosition);
            GUI.color = colour;
            if (!(pos2d.z > 0f)) return;
            int distance = (int)Vector3.Distance(Camera.main.transform.position, thePosition);
            string label = "<size=" + Settings["TextSize"] + ">" + name + " [" + distance + "m]</size>";
            var textDimensions = GUI.skin.box.CalcSize(new GUIContent(label));
            GUI.Box(new Rect(pos2d.x - 20f, Screen.height - pos2d.y - 20f, textDimensions.x, textDimensions.y), label);
        }
      
        static private void Noclip()
        {
            if (CheatsOn["Noclip"])
            {
                //    PlayerManager.localPlayer.GetComponent<FirstPersonController>().
                cFirstPersonController.noclip = true;
                Vector3 MyPos = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    MyPos += Camera.main.transform.forward;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    MyPos -= Camera.main.transform.forward;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    MyPos += Camera.main.transform.right;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    MyPos -= Camera.main.transform.right;
                }
                var speed = Settings["NoclipSpeed"];
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed *= 2.3f;
                }
                PlayerManager.localPlayer.transform.position += MyPos.normalized * speed * Time.deltaTime;
                if (Input.GetKey(KeyCode.Space))
                {
                    PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 5f;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 400f;
                    }
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 5f;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 400f;
                    }
                }
           //     cPlyMovementSync.CallCmdSyncData(PlayerManager.localPlayer.transform.rotation.eulerAngles.y, PlayerManager.localPlayer.transform.position, Camera.main.transform.rotation.eulerAngles.x);

            }
            else if(!CheatsOn["Freecam"])
            {
                cFirstPersonController.noclip = false;
            }
        }
        static private List<Door> ClosedDoors = new List<Door>();
        static private void AntiDoor()
        {
            if (CheatsOn["AntiDoor"])
            {
                foreach (Door door2 in UnityEngine.Object.FindObjectsOfType<Door>())
                {
                    float distance = Vector3.Distance(PlayerManager.localPlayer.transform.position, door2.transform.position);
                    if (!door2.isOpen && (distance < Settings["AntiDoorRange"]))
                    {
                        ClosedDoors.Add(door2);
                        door2.gameObject.SetActive(false);
                    }
                }
                foreach (Door door2 in ClosedDoors)
                {
                    if (door2 != null)
                    {
                        float distance = Vector3.Distance(PlayerManager.localPlayer.transform.position, door2.transform.position);
                        if (distance >= Settings["AntiDoorRange"])
                        {
                            door2.gameObject.SetActive(true);
                        }
                    }
                }

            }
            else
            {
                foreach (Door door2 in ClosedDoors)
                {
                    if (door2 != null) door2.gameObject.SetActive(true);
                }
                ClosedDoors.Clear();
            }
        }
        static private void SettingsMenu()
        {
            if (CheatsOn["SettingsMenu"])
            {
                CheatsOn["SettingsMenu"] = false;

                if (!Directory.Exists(directory))  // if it doesn't exist, create
                    Directory.CreateDirectory(directory);
                string filePath = directory + @"/Settings.txt";
                Process.Start(filePath);
                // combine the arguments together
                // it doesn't matter if there is a space after ','
                //      string argument = "/select, \"" + filePath + "\"";

                //         System.Diagnostics.Process.Start("explorer.exe", argument);
            }

        }
        static private void ListenAll()
        {
            if (CheatsOn["ListenAll"]) Radio.roundEnded = true;
            else Radio.roundEnded = false;
        }
        static private void Headless()
        {
            if(CheatsOn["Headless"] && !CheatsOn["Ballerina"])
            {
                cPlyMovementSync.CallCmdSyncData(PlayerManager.localPlayer.transform.rotation.eulerAngles.y, PlayerManager.localPlayer.transform.position, -590);
            }
        }



        private static float RecallTime = -99;
        private static Ragdoll TargetCorpse = null;
        private static GameObject TargetPlayer = null;

        private static bool sameteam(GameObject player)
        {
            try
            {
                if (cCharacterClassManager.klasy[player.GetComponent<CharacterClassManager>().curClass].team == cCharacterClassManager.klasy[cCharacterClassManager.curClass].team)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        static private void SCPMode()
        {
            if (CheatsOn["SCPMode"])
            {
                //  PlayerManager.localPlayer.GetComponent<InventoryDisplay>().isSCP = false;
                if (Settings["SCPAutoAttack"] == 0 && !Input.GetKeyDown(KeyCode.Mouse0)) return;
                if (cScp173PlayerScript.iAm173)
                {
                    cScp173PlayerScript.GetType().GetField("allowMove", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(cScp173PlayerScript, true);
                    foreach (GameObject player in PlayerManager.singleton.players)
                    {
                        if (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < (4f + cScp173PlayerScript.boost_teleportDistance.Evaluate(PlayerManager.localPlayer.GetComponent<PlayerStats>().GetHealthPercent())) && player != PlayerManager.localPlayer && !sameteam(player)) 
                        {
                            cScp173PlayerScript.CallCmdHurtPlayer(player);
                            Hitmarker.Hit(1f);
                        }
                    }
                }
                if (cScp049PlayerScript.iAm049)
                {
                    foreach (GameObject player in PlayerManager.singleton.players)
                    {
                        if ((player != PlayerManager.localPlayer) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 3.5f) && !sameteam(player))
                        {
                            cScp049PlayerScript.CallCmdInfectPlayer(player, "123");
                            Hitmarker.Hit(1f);
                        }
                    }

                    if(18 > Time.time - RecallTime && Time.time - RecallTime > 14)
                    {
                        cScp049PlayerScript.CallCmdRecallPlayer(TargetPlayer, TargetCorpse.gameObject);
                        RecallTime = -99;
                        TargetCorpse = null;
                        TargetPlayer = null;
                        cScp049PlayerScript.CallCmdAbortInfecting();
                    }
                    if (Time.time - RecallTime > 19)
                    {
                        foreach (var corpse in FindObjectsOfType<Ragdoll>())
                        {
                            if (corpse.allowRecall && (Vector3.Distance(PlayerManager.localPlayer.transform.position, corpse.transform.position) < 3))
                            {
                                foreach (GameObject player in PlayerManager.singleton.players)
                                {
                                    if (corpse.owner.ownerHLAPI_id == player.GetComponent<HlapiPlayer>().PlayerId)
                                    {
                                        cScp049PlayerScript.CallCmdStartInfecting(player, corpse.gameObject);
                                        RecallTime = Time.time;
                                        TargetPlayer = player;
                                        TargetCorpse = corpse;
                                    }
                                }
                            }
                        }
                    }

                }
                if (cScp049_2PlayerScript.iAm049_2)
                {
                    foreach (GameObject player in PlayerManager.singleton.players)
                    {
                        if ((player != PlayerManager.localPlayer) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 4f) && !sameteam(player)) cScp049_2PlayerScript.CallCmdHurtPlayer(player, "123");
                    }
                }
                if (cScp939PlayerScript.iAm939)
                {
                    foreach (GameObject player in PlayerManager.singleton.players)
                    {
                        if (!sameteam(player))
                        {
                            if (Vector3.Distance(Camera.main.transform.position, player.transform.position) < 1.3 * cScp939PlayerScript.attackDistance)
                            {
                                cScp939PlayerScript.CallCmdShoot(player);
                            }
                        }
                    }
                }
                if (cScp096PlayerScript.iAm096)
                {
                    foreach (GameObject player in PlayerManager.singleton.players)
                    {
                        if ((Vector3.Distance(Camera.main.transform.position, player.transform.position) < 3.5f) && !sameteam(player))
                        {
                            cScp096PlayerScript.CallCmdHurtPlayer(player);
                        }

                    }
                }
                if (cScp106PlayerScript.iAm106)
                {
                    foreach (GameObject player in PlayerManager.singleton.players)
                    {
                        if ((Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 2.7f) && player != PlayerManager.localPlayer && !sameteam(player))
                        {
                            cScp106PlayerScript.CallCmdMovePlayer(player, ServerTime.time);
                            Hitmarker.Hit(1f);
                        }
                    }
                }
                if (cScp079PlayerScript.iAm079)
                {
                    foreach (TeslaGate tesla in FindObjectsOfType<TeslaGate>())
                    {
                        foreach (var player in PlayerManager.singleton.players)
                        {
                            //if (Vector3.Distance(tesla.transform.position, player.transform.position) < new Vector3(1.7f, 6.5f, 1.7f))
                            Vector3 sizeOfKiller = new Vector3(1.7f, 6.5f, 1.7f);
                            Collider[] deathrange = Physics.OverlapBox(player.transform.position + Vector3.up * (sizeOfKiller.y / 2f), sizeOfKiller / 2f, default(Quaternion), tesla.killerMask);
                            foreach (Collider collider in deathrange)
                            {
                          //      cScp079PlayerScript.CallCmdSwitchCamera(tesla.transform.position, )
                                cScp079PlayerScript.CallCmdInteract("TESLA", tesla.gameObject);
                            }

                        }
                    }
                    //tesla gate aimbot
                }
            }
        }
        static private List<Tuple<string, Vector3, Color>> LocationsToRender = new List<Tuple<string, Vector3, Color>>();
        static private float LocationRefresh = -80;
        static private void LocationESP()
        {
            if (CheatsOn["LocationESP"] && (Time.time - LocationRefresh > Settings["LocationRefreshRate"]))
            {
                LocationsToRender.Clear();
                LocationRefresh = Time.time;
                Color LocationColour = new Color(1f, 0.65f, 1f, 0.6f);
                foreach (TeslaGate teslaGate in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
                {
                    LocationsToRender.Add(new Tuple<string, Vector3, Color>("Tesla Gate", teslaGate.transform.position, LocationColour));
                }
                foreach (PocketDimensionTeleport exit in UnityEngine.Object.FindObjectsOfType<PocketDimensionTeleport>())
                {
                    if (exit.GetTeleportType() == PocketDimensionTeleport.PDTeleportType.Exit) LocationsToRender.Add(new Tuple<string, Vector3, Color>("Exit (probably)", exit.transform.position, LocationColour));
                }
                foreach (Generator079 generator in UnityEngine.Object.FindObjectsOfType<Generator079>())
                {
                    LocationsToRender.Add(new Tuple<string, Vector3, Color>("Computer Generator", generator.transform.position, LocationColour));
                }
                foreach (GameObject elivator in UnityEngine.GameObject.FindGameObjectsWithTag("LiftTarget"))
                {
                    if (elivator.name.Contains("ElevatorChamber"))
                    {
                        LocationsToRender.Add(new Tuple<string, Vector3, Color>("Lift ", elivator.transform.position, LocationColour));
                    }
                }
                LocationsToRender.Add(new Tuple<string, Vector3, Color>("914", GameObject.FindGameObjectWithTag("914_use").transform.position, LocationColour));
                LocationsToRender.Add(new Tuple<string, Vector3, Color>("Intercom", UnityEngine.Object.FindObjectOfType<Intercom>().transform.position, LocationColour));
            }
            else if (!CheatsOn["LocationESP"])
            {
                LocationsToRender.Clear();
                LocationRefresh = -80;
            }
        }
        static private List<Tuple<string, Vector3, Color>> PlayersToRender = new List<Tuple<string, Vector3, Color>>();
        static private float RefreshPlayers;
        static private void PlayerESP()
        {
            PlayersToRender.Clear();
            if (CheatsOn["PlayerESP"])
            {
                Color PlayerColor = Color.gray;
                foreach (var player in PlayerManager.singleton.players)
                {
                    var nickname = player.transform.GetComponent<NicknameSync>();
                    var role = nickname.GetComponent<CharacterClassManager>().curClass;
                    if (role != -1)
                    {
                        Vector3 position = player.GetComponent<NetworkIdentity>().transform.position;
                        var rolename = nickname.GetComponent<CharacterClassManager>().klasy[role].fullName;
                        switch (role)
                        {
                            case 1:
                                PlayerColor = new Color(1f, 0.6f, 0f, 1f);
                                break;
                            case 15:
                                PlayerColor = Color.grey;
                                break;
                            case 0:
                            case 3:
                            case 5:
                            case 10:
                            case 9:
                            case 16:
                            case 17:
                            case 7:
                                PlayerColor = Color.red;
                                break;
                            case 14:
                                PlayerColor = new Color(1f, 1f, 0.7f, 1);
                                break;
                            case 8:
                                PlayerColor = Color.green;
                                break;
                            case 6:  // scientist
                                PlayerColor = Color.white;
                                break;
                            case 4:
                            case 11:
                            case 12:
                            case 13:
                                PlayerColor = Color.blue;
                                break;
                        }
                        var name = string.Format(nickname.myNick + "{0}" + rolename, Environment.NewLine);
                        PlayersToRender.Add(new Tuple<String, Vector3, Color>(name, new Vector3(position.x, position.y + 1, position.z), PlayerColor));
                        // PlayersToRender.Add(new Tuple<String, Vector3, Color>(" (" + rolename + ") ", new Vector3(position.x, position.y + 3, position.z), PlayerColor));
                    }
                }
            }
        }
        static private List<Tuple<string, Vector3, Color>> ItemsToRender = new List<Tuple<string, Vector3, Color>>();
        static private List<Pickup> items = new List<Pickup>();
        static private float ItemRefresh = 0;
        static private void ItemsESP()
        {
            ItemsToRender.Clear();
            if (CheatsOn["ItemsESP"])
            {
                if (!items.Any() || ((Time.time - ItemRefresh) > Settings["ItemRefreshRate"]))
                {
                    items = UnityEngine.Object.FindObjectsOfType<Pickup>().ToList();
                    ItemRefresh = Time.time;
                }
                Color ItemsColour = new Color(1f, 1f, 0.7f, 0.5f);
                foreach (Pickup item in items)
                {
        
                    if (item != null)
                    {
                        float distance = Vector3.Distance(PlayerManager.localPlayer.transform.position, item.transform.position);
                        if (distance < Settings["ItemESPRange"])
                        {
                            ItemsToRender.Add(new Tuple<string, Vector3, Color>(PlayerManager.localPlayer.GetComponent<Inventory>().availableItems[item.info.itemId].label, item.transform.position, ItemsColour));
                        }
                    }
                }
            }
            else
            {
                ItemRefresh = -100;
            }
            
        }
        static private Dictionary<WeaponManager.Weapon, float> Weapons = new Dictionary<WeaponManager.Weapon, float>();

        static private void Reload()
        {
            if (!Directory.Exists(directory))  // if it doesn't exist, create
                Directory.CreateDirectory(directory);



            try
            {
                List<string> KeyBindings = File.ReadAllLines(directory + @"/KeyBindings.txt").ToList();
                foreach (string line in KeyBindings)
                {
                    string[] values = line.Split('=');
                    CheatKeys[values[0]] = (KeyCode)System.Enum.Parse(typeof(KeyCode), values[1]);
                }
            }
            catch
            {
                using (StreamWriter writer = new StreamWriter(directory + @"/KeyBindings.txt", false))
                {
                    foreach (KeyValuePair<String, UnityEngine.KeyCode> state in CheatKeys)
                    {
                        writer.WriteLine(state.Key + "=" + state.Value.ToString());
                    }
                    writer.Flush();
                }
            }

            try
            {
                List<string> options = File.ReadAllLines(directory + @"/Settings.txt").ToList();
                foreach (string line in options)
                {
                    string[] values = line.Split('=');
                    Settings[values[0]] = float.Parse(values[1]);
                }
            }
            catch
            {
                using (StreamWriter writer = new StreamWriter(directory + @"/Settings.txt", false))
                {
                    foreach (KeyValuePair<String, float> state in Settings)
                    {
                        writer.WriteLine(state.Key + "=" + state.Value.ToString());
                    }
                    writer.Flush();
                }
            }
        }
        static private void Electrician()
        {
            if (CheatsOn["Electrician"])
            {
                foreach (TeslaGate Zapper in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
                {
                    Zapper.sizeOfKiller = new Vector3(0, 0, 0);
                }
            }
            else
            {
                foreach (TeslaGate Zapper in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
                {
                    Zapper.sizeOfKiller = new Vector3(1.7f, 6.5f, 1.7f);
                }
            }
        }
        static private void Eject()
        {
            if (CheatsOn["Eject"]) Injector.Unload();
        }
        static public string directory = Environment.ExpandEnvironmentVariables("%USERPROFILE%/Documents/LithiumCheatv6point5");
        private void Start()
        {
            Memory.Init();
            Reload();

            try
            {
                List<string> CheatState = File.ReadAllLines(directory + @"/DefaultCheatState.txt").ToList();
                foreach (string line in CheatState)
                {
                    string[] values = line.Split('=');
                    CheatsOn[values[0]] = bool.Parse(values[1]); //bool.Parse(values[1]);
                }
            }
            catch
            {
                using (StreamWriter writer = new StreamWriter(directory + @"/DefaultCheatState.txt", false))
                {
                    foreach (KeyValuePair<String, bool> state in CheatsOn)
                    {

                        writer.WriteLine(state.Key + "=" + state.Value);
                    }
                    writer.Flush();
                }
            }
            foreach (KeyValuePair<string, Action> entry in Cheats)
            {
                CheatsOn[entry.Key] = false;
            }

        }
        static private void ESPDisplay(string name, Vector3 thePosition, Color colour)
        {
            var pos2d = Camera.main.WorldToScreenPoint(thePosition);
            GUI.color = colour;
            if (!(pos2d.z > 0f)) return;
            GUI.Label(new Rect(pos2d.x - 20f, Screen.height - pos2d.y - 20f, pos2d.x + 40f, Screen.height - pos2d.y + 50f), name + " [" + (int)Vector3.Distance(Camera.main.transform.position, thePosition) + "m]");
        }

        private static float ReloadTime = 0;
        public void Update()
        {
           // Memory.install(12);
         //   var harmony = HarmonyInstance.Create("com.company.project.product");

            if (Input.GetKeyUp(CheatKeys["Aimbot"])) CheatsOn["Aimbot"] = false;
            if (Time.time - ReloadTime > Settings["txtLoadTime"])
            {
                Reload();
                ReloadTime = Time.time;
            }
            foreach (KeyValuePair<string, KeyCode> entry in CheatKeys)
            {
                if (Input.GetKeyDown(entry.Value))
                {
                    CheatsOn[entry.Key] = !CheatsOn[entry.Key];
                }
                try
                {
                    Cheats[entry.Key]();
                }
                catch
                {
                    try
                    {
                        RefreshComponents();

                    }
                    catch { }
                }
            }
        }
        static private string WarningMessage = "----------------------";

        public void OnGUI()
        {
            Memory.Hook();
            GUI.backgroundColor = new Color(54, 225, 168, 50);
            if (!CheatsOn["GlobalModWarning"]) WarningMessage = "<color=#00ff00><b><size=" + Settings["TextSize"] + ">No Moderators Detected</size></b></color>";
            var warningsize = GUI.skin.box.CalcSize(new GUIContent(WarningMessage));


            GUI.Box(new Rect((float)(Screen.width / 2 - 145), 20f, 40 + warningsize.x, warningsize.y), WarningMessage);
            var toRender = LocationsToRender.Concat(ItemsToRender).Concat(PlayersToRender);
            foreach (Tuple<string, Vector3, Color> label in toRender)
            {
                try
                {
                    DisplayLocation(label.Item1, label.Item2, label.Item3);
                }
                catch { };
            }
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            if (Settings["ShowCurAmmo"] == 1f)
            {
                try
                {
                    int curammo = cWeaponManager.AmmoLeft();
                    int outof = cWeaponManager.weapons[cWeaponManager.curWeapon].maxAmmo;
                    int totalammo = cAmmoBox.GetAmmo(cWeaponManager.weapons[cWeaponManager.curWeapon].ammoType);
                    string ammocount = "<color=#ffffff><b><size=" + Settings["TextSize"] * 2.5 + ">" + curammo.ToString() + @" / " + outof.ToString() + Environment.NewLine + totalammo.ToString() + "</size></b></color>";
                    var ammocountsize = GUI.skin.box.CalcSize(new GUIContent(ammocount));
                    GUI.Box(new Rect(Screen.width - 250, Screen.height - 120, ammocountsize.x, ammocountsize.y), ammocount);
                }
                catch
                {
                }
            }
            if (Settings["ShowMenu"] == 1)
            {


                string topleftmenu = "<color=#e8e272><b><size=" + Settings["TextSize"] + ">Lithium l33t_H4x5 v6.5</size></b></color>";
                topleftmenu += Environment.NewLine + "<color=#e8e272>" + "<b><size=" + Settings["TextSize"] + ">https://t.me/lithiumscp</size></b></color>";
                topleftmenu += Environment.NewLine + "<color=#e8e272><b><size=" + Settings["TextSize"] + ">──────────────────</size></b></color>";
               
                int RectOrder = 0;
                foreach (KeyValuePair<string, bool> entry in CheatsOn)
                {
                    RectOrder++;
                    topleftmenu += Environment.NewLine + "<size=" + Settings["TextSize"] + "><color=#ffffff>" + entry.Key + " </color><color=#94fff2>[" + CheatKeys[entry.Key] + "]</color>  " + (entry.Value ? "<color=#75d975>ON</color>" : "<color=#ffa6a6>OFF</color>") + "</size>";
                }
                var topleftsize = GUI.skin.box.CalcSize(new GUIContent(topleftmenu));
                GUI.Box(new Rect(20f, 45f, topleftsize.x + 30, topleftsize.y + 15), string.Empty);
                GUI.Label(new Rect(20f, 35f, topleftsize.x, topleftsize.y), topleftmenu);
            }

        }
    }
}