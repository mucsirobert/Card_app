using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vote  {
    public enum Options: byte
    {
        Yes = 0,
        No,
        NOTENDED
    }

    public class VoteData
    {
        public Player Player { get; private set; }
        public Options Option { get; private set; }

        public VoteData(Player player, Options option)
        {
            this.Player = player;
            this.Option = option;
        }
    }


    public Player Owner { get; private set; }

    public Vote(Player owner)
    {
        this.Owner = owner;
    }

    private List<VoteData> votes = new List<VoteData>();

    public bool Ended { get
        {
            return votes.Count == NetworkManager.singleton.numPlayers;
        }
    }

    public Options GetResult()
    {
        if (!Ended)
            return Options.NOTENDED;

        var players = Player.Players;
        int numberOfYeses = 0;

        /*foreach (var p in players)
        {
            if ()
        }*/
        foreach (var vote in votes)
        {
            if (vote.Option == Options.Yes)
                numberOfYeses++;
        }

        if (numberOfYeses == players.Count)
            return Options.Yes;
        else
            return Options.No;
    }

    public void AddVote(Player player, Options option)
    {
        votes.Add(new VoteData(player, option));
    }


}
