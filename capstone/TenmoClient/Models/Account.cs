﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    internal class Account
    {
        public int AccountId { get; set; }

        public int UserId { get; set; }

        public decimal Balance { get; set; }

    }
}