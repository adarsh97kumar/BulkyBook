﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.orderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var OrderFromDb = _db.orderHeaders.FirstOrDefault(u => u.Id == id);
            if(OrderFromDb != null)
            {
                OrderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    OrderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var OrderFromDb = _db.orderHeaders.FirstOrDefault(u => u.Id == id);
            OrderFromDb.PaymentDate = DateTime.Now;
            OrderFromDb.SessionId = sessionId;
            OrderFromDb.PaymentIntentId = paymentIntentId;
        }
    }
}
