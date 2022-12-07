import {POST_URL, CATEGORY_URL, HEADERS_CONFIG, TOKEN, RATE_URL, COMMENT_URL, DETOKEN, AUTHEN_API_URL} from './config.js'
const id = location.pathname.split("/")[3];

axios({
  method: "get",
  url: POST_URL + `/Detail/${id}`,
}).then(function (response) {
  renderPost(response.data);
  getComment();

  if (!TOKEN) {
    $("#rateBlock").addClass("hide");
    $("#addComment").addClass("hide");
    $("#loginToComment").removeClass("hide");
  } else {
    $("#rateBlock").removeClass("hide");
    $("#addComment").removeClass("hide");
    $("#loginToComment").addClass("hide");
    ratePost();
    getRate();
  }
});

function renderPost(post) {
  const postBlock = document.getElementById("postBlock");
  let html = `<div class="card border-info">
            <div class="card-body">
                <h5 class="card-title">${post.title}</h5>
                <div class="row">
                    <div id="cate" class="col-md-6">
                    </div>
                    <div class="col-md-6 text-end">
                        <small id="rateInfo" class="card-text text-muted px-5">Rate: ${
                          post.rate
                        } (${post.rateCount}) - View: ${post.views}</small>
                    </div>
                </div>
                <p class="card-text mt-3">${post.content}</p>
                <p class="card-text text-muted mt-3">Published at: ${convertDate(
                  post.createdAt
                )}</p>
            </div>
        </div>`;

  getCate(post.categoryId);
  getOtherPost(post.id, post.categoryId);
  postBlock.innerHTML = html;
  document.title = post.title;
}

function convertDate(date) {
  date = new Date(date);
  return date;
}

function getCate(id) {
  axios.get(CATEGORY_URL + `/${id}`).then(function (response) {
    $("#cate").prepend(
      `<a href="/Categories/Detail/${response.data.id}" class="btn btn-outline-warning">${response.data.title}</a>`
    );
  });
}

function getOtherPost(postId, cateId) {
  axios
    .get(POST_URL + `/Other?postId=${postId}&cateId=${cateId}`)
    .then(function (response) {
      renderPosts(response.data);
    });
}

function renderPosts(posts) {
  const other = document.getElementById("other");
  const htmls = posts.map(function (post) {
    return `<div class="card mb-3">
    <div class="card-header">
        <a id="detail" href="/Posts/Detail/${
          post.id
        }" data-id=${post.id} class="text-decoration-none">${post.title}</a>
    </div>
    <div class="card-body">
        <div id="cate-${post.id}">
        <p hidden>${getCates(post.id, post.categoryId)}</p>
        </div>
        <p class="card-text">${post.description.length > 100 ? post.description.slice(0, 100) + "..." : post.description}</p>
        <p class="card-text">Rate: ${
          post.rate
        } (${post.rateCount})  - View: ${post.views}</p>
        <a id="moreInfo" href="/Posts/Detail/${
          post.id
        }" class="btn btn-primary">More info</a>
    </div>
</div>`;
  });

  other.innerHTML = htmls.join("");
}

function getCates(postid, id) {
  axios.get(CATEGORY_URL + `/${id}`).then(function (response) {
    $(`#cate-${postid}`).prepend(
      `<a href="/Categories/Detail/${response.data.id}" class="btn btn-outline-warning">${response.data.title}</a>`
    );
  });
}

function ratePost() {
  $("input[type='radio']").click(() => {
    let totalRate = $("input[name='TotalRate']:checked").val();
    axios({
      method: "POST",
      url: RATE_URL,
      data: {
        postId: id,
        totalRate: totalRate,
        userId: DETOKEN.UserId,
      },
      headers: HEADERS_CONFIG
    });
  });
}

function getRate() {
  axios({
    method: "Get",
    url: RATE_URL + `/${id}/${DETOKEN.UserId}`,
    headers: HEADERS_CONFIG
  }).then((res) => {
    displayRate(res.data.totalRate);
  });
}

let displayRate = (totalRate) => {
  const rateItemList = document.getElementsByClassName("rate-item");
  const rateArr = Object.values(rateItemList).reverse();
  rateArr.forEach((item, index) => {
    if (index < totalRate) {
      item.checked = true;
    }
  });
};

$("#sendComment").click(function () {
  axios({
    method: "POST",
    url: COMMENT_URL,
    data: {
      postId: id,
      userId: DETOKEN.UserId,
      commentContent: $("#commentContent").val(),
    },
    headers: HEADERS_CONFIG
  }).then((res) => {
    renderComment(res.data);
  });
});

let renderComment = (comment) => {
  $("#comments").prepend(`
    <div><div class="input-group mb-3">
        <input type="hidden" id="commentId" value="${comment.id}">
        <span class="input-group-text fw-bold" id="basic-addon3">User</span>
        <input type="text" class="form-control" id="basic-url" aria-describedby="basic-addon3" value="${
          comment.commentContent
        }" disabled>
        <button id="${
          comment.id
        }" name="delete" class="btn btn-danger">Delete</button>
    </div>
    <p class="text-muted">${convertDate(comment.createdAt)}</p></div>
    `);
  displayDelete();
};

let getComment = () => {
  axios({
    method: "GET",
    url: COMMENT_URL +`/${id}`,
  }).then((res) => {
    renderComments(res.data);
  });
};

let renderComments = (comments) => {
  const commentBlock = document.getElementById("comments");
  const htmls = comments.map((comment) => {
    return `<div><div class="input-group mb-3">
    <input type="hidden" id="commentId" value="${comment.id}">
    <span id="showUsername-${comment.id}" class="input-group-text fw-bold" id="basic-addon3"><p class="hide">${getUserName(comment.id,comment.userId)}</p></span>
    <input type="text" class="form-control" id="basic-url" aria-describedby="basic-addon3" value="${
      comment.commentContent
    }" disabled>
    <button id="${
      comment.id
    }" name="delete" class="btn btn-danger">Delete</button>
    </div>
    <p class="text-muted">${convertDate(comment.createdAt)}</p></div>`;
  });

  commentBlock.innerHTML = htmls;
  displayDelete();
};

let displayDelete = () => {
  $("button[name='delete']").addClass("hide");

  if (DETOKEN.Roles === "Admin") {
    $("button[name='delete']").removeClass("hide");
  } else {
    $("button[name='delete']").addClass("hide");
  }
};

$("#comments").click((e) => {
  var deleteBtn = $(e.target)[0].id;
  axios({
    method: "Delete",
    url: COMMENT_URL+ `/${deleteBtn}`,
    headers: HEADERS_CONFIG
  }).then((res) => {
    document.getElementById(deleteBtn).parentElement.parentElement.innerHTML = "";
  });
});

let getUserName = (commentId,userId) => {
  axios({
    method: "GET",
    url: AUTHEN_API_URL + `/${userId}`,
  }).then((res)=>{
    $(`#showUsername-${commentId}`).text(res.data)
  })
}