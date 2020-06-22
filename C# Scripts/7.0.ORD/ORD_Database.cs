using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ORD_Database
{
    int need;

    //Common
    int selectionWisp;
    int cLuffy;
    int cZoro;
    int cNami;
    int cUsopp;
    int cSanji;
    int cChopper;
    int cSword;
    int cGun;
    int cBuggy;

    //Uncommon
    int ucFukuro;
    int ucBlueno;
    int ucRobin;
    int ucBepo;
    int ucBrook;
    int ucSmoker;
    int ucAce;
    int ucInazuma;
    int ucUsopp;
    int ucChopper;
    int ucPerona;
    int ucFranky;
    int ucHachi;

    //Special
    int sXdrake;
    int sEnel;
    int sMoria;
    int sNami;
    int sLucci;
    int sRobin;
    int sLaw;
    int sLuffy;
    int sMarco;
    int sBasil;
    int sBuggy;
    int sBonkurei;
    int sSanji;
    int sSquard;
    int sArlong;
    int sAbsalom;
    int sAce;
    int sUsopp;
    int sInazuma;
    int sZoroGhost;
    int sZoroSupernova;
    int sJinbe;
    int sChaka;
    int sChopperBrain;
    int sChopperJump;
    int sCapone;
    int sKuma;
    int sCrocodile;
    int sKid;
    int sKiller;
    int sTashigi;
    int sDoking;
    int sHelmeppo;

    //Rare
    int rEnel;
    int rBlack;
    int rDoflamingo;
    int rLaw;
    int rLuffy;
    int rRyuma;
    int rMarco;
    int rMagellan;
    int rMomonga;
    int rMihawk;
    int rBartol;
    int rBurgess;
    int rBasil;
    int rDecken;
    int rBeckman;
    int rBrook;
    int rVivi;
    int rSabo;
    int rSanji;
    int rShanks;
    int rSentomaru;
    int rSugar;
    int rSmoker;
    int rShiliew;
    int rAokiji;
    int rAkainu;
    int rOars;
    int rUsopp;
    int rIvankov;
    int rZeff;
    int rZoro;
    int rJozu;
    int rChopper;
    int rKaku;
    int rCrocodile;
    int rKid;
    int rKizaru;
    int rKinemon;
    int rPerona;
    int rFranky;
    int rHancock;

    //Lagendary
    int lGarp;
    int lBlack;
    int lMoria;
    int lNami;
    int lDragon;
    int lLaboon;
    int lReiju;
    int lRayleigh;
    int lLucci;
    int lLaw;
    int lLuffy;
    int lLuffyNightmare;
    int lMarco;
    int lBartol;
    int lSanji;
    int lShanks;
    int lSengoku;
    int lSugar;
    int lCaesar;
    int lShiki;
    int lAce;
    int lZ;
    int lZeff;
    int lZoro;
    int lJinbe;
    int lChopper;
    int lKalgara;
    int lKoby;
    int lKuma;
    int lHancock;
    int lFujitora;
    int lWhite;
    int lHiluluk;

    //Changed
    int chDoflamingo;
    int chBeckman;
    int chVivi;
    int chAce;
    int chKoby;
    int chCarrot;

    //Hidden
    int hRedforce;
    int hRebecca;
    int hRyuma;
    int hMobydick;
    int hMihawk;
    int hDecken;
    int hBaratie;
    int hMaxim;
    int hVergo;
    int hBonkurei;
    int hSabo;
    int hShiliew;
    int hSunny;
    int hAokiji;
    int hAkainu;
    int hIvankov;
    int hCarrot;
    int hKoala;
    int hKinemon;
    int hKiller;
    int hPerona;
    int hTiger;

    //Transcendence
    int trBlack;
    int trNami;
    int trDoflamingo;
    int trRobin;
    int trLaw;
    int trLucci;
    int trLuffy;
    int trBrook;
    int trBigmam;
    int trSabo;
    int trSanji;
    int trShanks;
    int trShirahoshi;
    int trAokiji;
    int trAkainu;
    int trUsopp;
    int trZoro;
    int trChopper;
    int trKizaru;
    int trTashigi;
    int trFranky;
    int trFujitora;

    //Immortal
    int imGarp;
    int imDragon;
    int imRayleigh;
    int imRoger;
    int imSengoku;
    int imGaban;
    int imShiki;
    int imZ;
    int imWhite;

    //Eternal
    int etAce;
    int etBuggy;
    int etHancock;
    int etCavendish;
    int etVivi;
    int etMihawk;

    //업그레이드
    public void Upgrade(List<int> material, int result)
    {
        for(int i=0; i < material.Count; i++)
        {
            material[i]--;
        }
        result++;
    }
    //업그레이드 함수 예시
    /*
    public void cLuffyUpgrade()
    {
        List<int> cLuffyList = new List<int>();

        if (selectionWisp >= 1)
        {
            cLuffyList.Add(selectionWisp);
        } else
        {
            need++;
        }

        if(need == 0)
        {
            Upgrade(cLuffyList, cLuffy);
        }

        need = 0;
    }*/
}