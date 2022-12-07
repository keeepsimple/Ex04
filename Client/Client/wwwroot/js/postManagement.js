import {POST_MANAGEMENT_URL, HEADERS_CONFIG, CATEGORY_MANAGEMENT_URL} from './config.js'

let renderTable = (posts) => {
  let htmls = posts.map((post) => {
    return `
      <tr>
          <th>${
            post.title.length > 20
              ? post.title.slice(0, 20) + "..."
              : post.title
          }</th>
          <th>${
            post.description.length > 20
              ? post.description.slice(0, 20) + "..."
              : post.description
          }</th>
          <th>${post.rate}</th>
          <th>${post.views}</th>
          <th>
              <button id="edit" class="btn btn-outline-warning btn-icon-split" data-bs-toggle="modal" data-bs-target="#editModal">Edit
              </button>
              <button id="delete" class="btn btn-outline-danger btn-icon-split" data-bs-toggle="modal" data-bs-target="#deleteModal">Delete
              </button>
              <input type="hidden" name="id" id="id" value="${post.id}" />
          </th>
      </tr>`;
  });

  let thead = `<table class="table table-striped">
    <thead>
        <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Rate</th>
            <th>View</th>
            <th>Action</th>
        </tr>
    </thead>
    <tbody class="list-item">`;
  let endTable = `</tbody></table>`;
  document.getElementById("tableBlock").innerHTML =
    thead + htmls.join("") + endTable;

  $("table #edit").on("click", (e) => {
    var id = $(e.target).parent().find("#id").val();
    getPost(id, renderEdit);
  });

  $("table #delete").on("click", (e) => {
    var id = $(e.target).parent().find("#id").val();
    getPost(id, renderDelete);
  });
};

const getDataList = (item, searchString) => {
  let pageNum = item;
  let pageSize = 4;
  axios({
    method: "get",
    url:
      POST_MANAGEMENT_URL +
      `/GetPagination/${pageNum}?searchString=${searchString}&pageSize=${pageSize}`,
    headers: HEADERS_CONFIG
  }).then((response) => {
    renderTable(response.data.list);
    renderPaging(response.data.count, item, pageSize);
  });
};

getDataList(1, "");

const renderPaging = (count, pageNum, pageSize) => {
  const pageRange = 3;
  const totalPages = Math.ceil(count / pageSize);
  const pagingBlock = document.getElementById("paging");
  const previous = `<li id="prev" class="page-item"><a class="page-link" href="" onclick="module.changePage(event,${
    pageNum - 1
  })">Previous</a></li>`;
  const next = `<li id="next" class="page-item"><a class="page-link" href="" onclick="module.changePage(event,${
    pageNum + 1
  })">Next</a></li>`;

  let html = "";
  for (let i = 1; i <= totalPages; i++) {
    if (i >= pageNum - pageRange && i <= pageNum + pageRange) {
      html += `<li class="page-item ${
        i == pageNum ? "active" : ""
      }"><a class="page-link" href="" onclick="module.changePage(event,${i})">${i}</a></li>`;
    }
  }
  pagingBlock.innerHTML = previous + html + next;
  pageNum == 1
    ? $("li#prev").addClass("disabled")
    : $("li#prev").removeClass("disabled");
  pageNum + 1 > totalPages
    ? $("li#next").addClass("disabled")
    : $("li#next").removeClass("disabled");
};

export const changePage = (event, item) => {
  event.preventDefault();
  let search = $("input[id='searchString']").val();
  if (search == null) search = "";
  getDataList(item, search);
};

$("#searchBtn").click(() => {
  let search = $("input[id='searchString']").val();
  getDataList(1, search);
});

let getPost = (id, callBack) => {
  axios({
    method: "Get",
    url: POST_MANAGEMENT_URL + `/${id}`,
    headers: HEADERS_CONFIG,
  })
    .then((res) => {
      return res.data;
    })
    .then(callBack);
};

let renderEdit = (post) => {
  $("#edit-form input[id='id']").val(post.id);
  $("#edit-form input[name='Title']").val(post.title);
  $("#edit-form input[name='Description']").val(post.description);
  $("#edit-form textarea[name='Content']").val(post.content);
  $("#edit-form input[id='rate']").val(post.rate);
  $("#edit-form input[id='rateCount']").val(post.rateCount);
  $("#edit-form input[id='views']").val(post.views);
  getCategories((cate) => renderCateEdit(cate, post.categoryId));
};

let renderDelete = (post) => {
  $("#delete-form input[id='id']").val(post.id);
  $("#delete-form input[name='Title']").val(post.title);
};

let getCategories = (callBack) => {
  axios({
    method: "GET",
    url: CATEGORY_MANAGEMENT_URL,
    headers: HEADERS_CONFIG,
  })
    .then((res) => {
      return res.data;
    })
    .then(callBack);
};

let renderCateCreate = (categories) => {
  const htmls = categories.map((category) => {
    return `<option value="${category.id}">${category.title}</option>`;
  });

  $("select#CreateCateIds").html(htmls.join(""));
};

let renderCateEdit = (categories, cateId) => {
  const htmls = categories.map((category) => {
    return `<option value="${category.id}" ${
      cateId === category.id ? "selected" : ""
    }>${category.title}</option>`;
  });

  $("select#EditCateIds").html(htmls.join(""));
};

$("#edit-form").submit(() => {
  let title = $("#edit-form input[name='Title']").val();
  let description = $("#edit-form input[name='Description']").val();
  let content = $("#edit-form textarea[name='Content']").val();
  let id = $("#edit-form input[id='id']").val();
  let rate = $("#edit-form input[id='rate']").val();
  let rateCount = $("#edit-form input[id='rateCount']").val();
  let views = $("#edit-form input[id='views']").val();
  let cate = $("select#EditCateIds").find(":selected").val();
  axios({
    method: "PUT",
    url: POST_MANAGEMENT_URL,
    data: {
      id: id,
      title: title,
      description: description,
      content: content,
      rate: rate,
      rateCount: rateCount,
      views: views,
      categoryId: cate
    },
    headers: HEADERS_CONFIG,
  }).then(getDataList(1));
});

$("#delete-form").submit(() => {
  let id = $("#delete-form input[id='id']").val();
  axios({
    method: "DELETE",
    url: POST_MANAGEMENT_URL + `/${id}`,
    headers: HEADERS_CONFIG,
  }).then(getDataList(1));
});

$("#createBtn").click(() => {
  getCategories(renderCateCreate);
});

$("#create-form").submit(() => {
  let title = $("#create-form input[name='Title']").val();
  let desc = $("#create-form input[name='Description']").val();
  let content = $("#create-form textarea[name='Content']").val();
  let cate = $("select#CreateCateIds").find(":selected").val();
  axios({
    method: "POST",
    url: POST_MANAGEMENT_URL,
    data: {
      title: title,
      description: desc,
      content: content,
      categoryId: cate,
      rate: 0,
      rateCount: 0,
      views: 0,
    },
    headers: HEADERS_CONFIG,
  });
});
