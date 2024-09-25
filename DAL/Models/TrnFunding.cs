﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("trn_funding")]

    public class TrnFunding
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [ForeignKey("User")]
        [Column("lender_id")]
        public string LenderId { get; set; }

        [Required]
        [ForeignKey("Loans")]
        [Column("loan_id")]
        public string LoanId { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column("funded_at")]
        public DateTime FundedAt { get; set; }

        public MstUser User { get; set; }

        public MstLoans Loans { get; set; }
    }
    }
