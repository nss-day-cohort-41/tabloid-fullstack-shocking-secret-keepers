﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tabloid.Models;
using Tabloid.Repositories;

namespace Tabloid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            
        }

        [HttpGet("subscribedposts/{id}")]
        public IActionResult GetAllSubscribedPostsByUser(int id)
        {
            return Ok(_subscriptionRepository.GetSubscribedPostsForUser(id));
        }

        [HttpGet("{id}/getby/{authorId}")]
        public IActionResult GetSubscriptionById(int id, int authorId)
        {
           
            return Ok(_subscriptionRepository.GetSubscriptionByUserId(id, authorId));
        }

        [HttpPost]
        public IActionResult Post(Subscription subscription)
        {
            _subscriptionRepository.AddSubscription(subscription);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Subscription subscription)
        {
          
            _subscriptionRepository.UpdateSubscription(subscription);
            return NoContent();
        }

    }
}