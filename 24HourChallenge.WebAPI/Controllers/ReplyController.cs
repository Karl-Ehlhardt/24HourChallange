﻿using _24HourChallenge.Data;
using _24HourChallenge.Models;
using _24HourChallenge.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace _24HourChallenge.WebAPI.Controllers
{
    public class ReplyController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        //private method to create a comment service
        private ReplyService CreateReplyService()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            ReplyService replyService = new ReplyService(userId);
            return replyService;
        }

        //CRUD METHODS

        //========CREATE========//

        //POST
        public IHttpActionResult Post(CreateReply comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //instantitate service
            ReplyService service = CreateReplyService();

            if (!service.CreateReply(comment))
            {
                return InternalServerError();
            }

            return Ok();
        }


        ////GET BY ID
        public IHttpActionResult GetByCommentId(int id)
        {
            ReplyService service = CreateReplyService();

            IEnumerable<ReplyListItem> comments = service.GetReplyByCommentId(id);

            return Ok(comments);
        }

        //=========Read===========//

        //UPDATE

        [HttpPut]
        public async Task<IHttpActionResult> UpdateReply([FromUri] int id, [FromBody] ReplyListItem model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reply reply = await _context.Replies.FindAsync(id);

            if (reply == null)
            {
                return NotFound();
            }

            reply.Text = model.Text;


            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok();
            }

            return InternalServerError();

        }

        //DELETE

        [HttpDelete]
        public IHttpActionResult DeleteReply(int id)
        {

            var service = CreateReplyService();

            if (!service.DeleteRepliesById(id)) { return InternalServerError(); }

            return Ok();

        }
    }
}
