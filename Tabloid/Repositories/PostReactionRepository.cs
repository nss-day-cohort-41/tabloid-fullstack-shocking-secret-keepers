﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Tabloid.Models;

namespace Tabloid.Repositories
{

    public class PostReactionRepository : BaseRepository, IPostReactionRepository

    {
        public PostReactionRepository(IConfiguration config) : base(config) { }

        public List<PostReaction> GetAllPostReactions()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT pr.Id As PostReactionId, pr.PostId, pr.ReactionId, pr.UserProfileId AS UserProfileIdReactingToPost, 
                        r.Id As ReactionId, r.Name, r.ImageLocation 
                        FROM PostReaction pr
                        JOIN  Reaction r
                        ON pr.ReactionId = r.Id
                        
                       ";
                    

                    var postReactions = new List<PostReaction>();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PostReaction postReaction = new PostReaction
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("PostReactionId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            ReactionId = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileIdReactingToPost")),
                            Reaction = new Reaction
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
                            }
                        };

                        postReactions.Add(postReaction);

                    }

                    reader.Close();

                    return postReactions;
                }
            }
        }

        public List<PostReaction> GetAllReactionsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT pr.Id, pr.PostId, pr.ReactionId, pr.UserProfileId AS UserProfileIdReactingToPost, 
                        r.Id, r.Name, r.ImageLocation 
                        FROM PostReaction pr
                        JOIN  Reaction r
                        ON pr.ReactionId = r.Id
                        WHERE pr.PostId = @id
                       ";
                    cmd.Parameters.AddWithValue("@id", id);

                    var postReactions = new List<PostReaction>();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PostReaction postReaction = new PostReaction
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            ReactionId = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileIdReactingToPost")),
                            Reaction = new Reaction
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
                            }
                        };

                        postReactions.Add(postReaction);

                    }

                    reader.Close();

                    return postReactions;
                }
            }
        }

        //if reaction id exists in previous post reaction.reactionId then increment count on previous post reaction
        //if reaction id does not exist in previous post reaction then create a new post reaction with a count of 1
        public List<PostReaction> GetAllReactionsCountedByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                     SELECT pr.Id As PostReactionId, pr.PostId, pr.ReactionId, pr.UserProfileId AS UserProfileIdReactingToPost, 
                        r.Id, r.Name, r.ImageLocation 
                        FROM PostReaction pr
                        JOIN  Reaction r
                        ON pr.ReactionId = r.Id
                        WHERE pr.PostId = @id
                       ";
                    cmd.Parameters.AddWithValue("@id", id);

                    var postReactions = new List<PostReaction>();

                    var reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        PostReaction postReaction = new PostReaction
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("PostReactionId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            ReactionId = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileIdReactingToPost")),
                            Reaction = new Reaction
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation")),
                                ReactionCount = 1
                            }
                        };


                        var reactionId = reader.GetInt32(reader.GetOrdinal("ReactionId"));
                        var anotherPostReaction = postReactions.Find(pr => pr.ReactionId == reactionId);

                        if (anotherPostReaction == null) {

                            postReactions.Add(postReaction);

                        } else {

                            int initalReactionIndex = postReactions.FindIndex(pr => pr.ReactionId == reactionId);
                            postReactions[initalReactionIndex].Reaction.ReactionCount = postReactions[initalReactionIndex].Reaction.ReactionCount + 1;

                        }
                    }
                        reader.Close();


                    return postReactions;
                }
            }
        }

        public void AddPostReaction(PostReaction postReaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostReaction (PostId, ReactionId, UserProfileId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@postId, @reactionId, @userProfileId)";
                    cmd.Parameters.AddWithValue("@postId", postReaction.PostId);
                    cmd.Parameters.AddWithValue("@reactionId", postReaction.ReactionId);
                    cmd.Parameters.AddWithValue("@userProfileId", postReaction.UserProfileId);

                    int id = (int)cmd.ExecuteScalar();

                    postReaction.Id = id;
                }
            }
        }


    }
}


