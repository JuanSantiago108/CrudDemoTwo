// Using statements
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrudDemoTwo.Models;
namespace CrudDemoTwo.Controllers;

public class PostController : Controller
{
    private MyContext _context;

    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }

    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }

    // here we can "inject" our context service into the constructor
    public PostController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("/posts/all")]
    public IActionResult All()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        List<Post> AllPosts = _context.Posts.ToList();

        return View("All", AllPosts);
    }

    [HttpGet("/posts/new")]
    public IActionResult New()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        return View("NewPost");
    }


    [HttpPost("/posts/create")]
    public IActionResult Create(Post newPost)
    {

        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        if (ModelState.IsValid == false)
        {
            return New();
        }

        _context.Posts.Add(newPost);

        _context.SaveChanges();

        return RedirectToAction("All");
    }


    [HttpGet("/posts/{postId}")]
    public IActionResult ViewPost(int postId)
    {

        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        Post? post = _context.Posts.FirstOrDefault(post => post.PostId == postId);

        if (post == null)
        {
            return RedirectToAction("All");
        }

        return View("ViewPost", post);
    }

    [HttpPost("/posts/{deletedPostId}/delete")]
    public IActionResult Delete(int deletedPostId)
    {

        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        Post? postToBeDeleted = _context.Posts.FirstOrDefault(post => post.PostId == deletedPostId);

        if (postToBeDeleted != null)
        {
            _context.Posts.Remove(postToBeDeleted);
            _context.SaveChanges();
        }

        return RedirectToAction("All");
    }
    [HttpGet("/posts/{postTobeEdited}/edit")]


    public IActionResult EditPost(int postTobeEdited)
    {


        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }

        Post? post = _context.Posts.FirstOrDefault(post => post.PostId == postTobeEdited);

        if (post == null)
        {
            return RedirectToAction("All");
        }
        return View("Edit", post);
    }



    [HttpPost("/posts/{updatedPostId}/update")]
    public IActionResult UpdatePost(int updatedPostId, Post updatedPost)
    {

        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }


        if (ModelState.IsValid == false)
        {
            return EditPost(updatedPostId);
        }
        Post? dbPost = _context.Posts.FirstOrDefault(post => post.PostId == updatedPostId);

        if (dbPost == null)
        {
            return RedirectToAction("All");
        }

        dbPost.Topic = updatedPost.Topic;
        dbPost.Body = updatedPost.Body;
        dbPost.ImageUrl = updatedPost.ImageUrl;
        dbPost.UpdatedAt = DateTime.Now;

        _context.Posts.Update(dbPost);
        _context.SaveChanges();

        return RedirectToAction("All", new { postId = dbPost.PostId });
    }
}