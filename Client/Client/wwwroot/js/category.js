import {CATEGORY_URL} from './config.js'
const id = location.pathname.split("/")[3];

function getDataList(item) {
    let pageNum = item;
    let pageSize = 4;
    axios({
      method: "get",
      url: CATEGORY_URL + `/Details/${id}?pageNum=${pageNum}&pageSize=${pageSize}`,
    }).then(function (response) {
      renderPosts(response.data.list);
      renderPaging(response.data.count, item, pageSize);
    });
  }
  
  getDataList(1);
  
  export const changePage = (event, item) => {
    event.preventDefault();
    getDataList(item);
  }
  
  function renderPosts(posts) {
    const postBlock = document.getElementById("postBlock");
    const htmls = posts.map(function (post) {
      return `<div class="card mb-3">
      <div class="card-header">
          <a id="detail" href="/Posts/Detail/${
            post.id
          }" data-id=${post.id} class="text-decoration-none">${post.title}</a>
      </div>
      <div class="card-body">
          <div id="cate-${post.id}">
          <p hidden>${getCate(post.id, post.categoryId)}</p>
          </div>
          <p class="card-text">${post.description.length > 100 ? post.description.slice(0, 100) + "..." : post.description}</p>
          <p class="card-text">Rate: ${
            post.rate
          }(${post.rateCount})  - View: ${post.views}</p>
          <a id="moreInfo" href="/Posts/Detail/${
            post.id
          }" class="btn btn-primary">More info</a>
      </div>
  </div>`;
    });
  
    postBlock.innerHTML = htmls.join("");
  }
  
  function renderPaging(count, pageNum, pageSize) {
    const pageRange = 3;
    const totalPages = Math.ceil(count / pageSize);
    const pagingBlock = document.getElementById("paging");
    const previous = `<li id="prev" class="page-item"><a class="page-link" href="" onclick="module.changePage(event,${pageNum - 1})">Previous</a></li>`;
    const next = `<li id="next" class="page-item"><a class="page-link" href="" onclick="module.changePage(event,${pageNum + 1})">Next</a></li>`;
   
    let html= "";
    for (let i = 1; i <= totalPages; i++) {
      if (i >= pageNum - pageRange && i <= pageNum + pageRange) {
        html += `<li class="page-item ${i == pageNum ? "active" : ""}"><a class="page-link" href="" onclick="module.changePage(event,${i})">${i}</a></li>`;
      }
    }
    pagingBlock.innerHTML = previous + html + next
    pageNum == 1
    ? $("li#prev").addClass("disabled")
    : $("li#prev").removeClass("disabled");
    pageNum + 1 > totalPages
    ? $("li#next").addClass("disabled")
    : $("li#next").removeClass("disabled");
  }

  function getCate(postid, id) {
    axios.get(CATEGORY_URL + `/${id}`).then(function (response) {
      $(`#cate-${postid}`).prepend(
        `<a href="/Categories/Detail/${response.data.id}" class="btn btn-outline-warning">${response.data.title}</a>`
      );
      document.title = response.data.title;
    });
    
  }