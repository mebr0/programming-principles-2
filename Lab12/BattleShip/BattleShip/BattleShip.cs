﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class main : Form
    {
        Game game;

        public main()
        {
            InitializeComponent();

            game = new Game(PlayerType.human, PlayerType.bot);

            Controls.Add(game.player1);
            Controls.Add(game.player2);
        }
    }
}