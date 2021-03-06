﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
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

        [HttpGet("{id}")]
        public IActionResult GetAllSubscriptionsByUserId(int id)
        {
            return Ok(_subscriptionRepository.GetAllSubscriptionsByUserId(id));
        }

        [HttpPost("tertiary/{id}")]
        public IActionResult GetAllTertiarySubscriptionsByUserId(int id)
        {
            
           
                List<Subscription> secondarySubs = _subscriptionRepository.GetAllSubscriptionsByUserId(id);
            List<int> tertiarySubs = new List<int>();
               foreach(Subscription subscription in secondarySubs)
                {
                tertiarySubs.Add(subscription.ProviderUserProfileId);
                    
                }
            
            return Ok(tertiarySubs);
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

        [HttpPut("unsubscribe/{id}")]
        public IActionResult Put(int id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }

            _subscriptionRepository.UpdateSubscription(subscription);
            return NoContent();
        }

        [HttpPut("reactivate/{id}")]
        public IActionResult Reactivate(int id)
        {

            _subscriptionRepository.ReactivateSubscription(id);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _subscriptionRepository.DeleteSubscription(id);
            //return status 204
            return NoContent();
        }

    }
}
