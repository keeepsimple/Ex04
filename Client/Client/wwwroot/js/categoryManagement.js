import {CATEGORY_MANAGEMENT_URL, HEADERS_CONFIG} from './config.js'

let renderTable = (categories) => {
  let htmls = categories.map((category) => {
    return `
    <tr>
        <th>${category.title}</th>
        <th>${category.description}</th>
        <th>
            <button id="edit" class="btn btn-outline-warning btn-icon-split" data-bs-toggle="modal" data-bs-target="#editModal">Edit
            </button>
            <button id="delete" class="btn btn-outline-danger btn-icon-split" data-bs-toggle="modal" data-bs-target="#deleteModal">Delete
            </button>
            <input type="hidden" name="id" id="id" value="${category.id}" />
        </th>
    </tr>`;
  });

  let thead = `<table class="table table-striped">
  <thead>
      <tr>
          <th>Title</th>
          <th>Description</th>
          <th>Action</th>
      </tr>
  </thead>
  <tbody class="list-item">`;
  let endTable = `</tbody></table>`;
  document.getElementById("tableBlock").innerHTML =
    thead + htmls.join("") + endTable;

  $("table #edit").on("click", (e) => {
    var id = $(e.target).parent().find("#id").val();
    getCate(id, renderEdit);
  });

  $("table #delete").on("click", (e) => {
    var id = $(e.target).parent().find("#id").val();
    getCate(id, renderDelete);
  });
};

const getDataList = (item, searchString) => {
  let pageNum = item;
  let pageSize = 4;
  axios({
    method: "get",
    url:
    CATEGORY_MANAGEMENT_URL + `/GetPagination/${pageNum}?searchString=${searchString}&pageSize=${pageSize}`,
    headers: HEADERS_CONFIG
  }).then((response) => {
    renderTable(response.data.list);
    renderPaging(response.data.count, item, pageSize);
  });
};

getDataList(1, "")

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
  if(search == null) search = "";
  getDataList(item, search);
};

let getCate = (id, callBack) => {
  axios({
    method: "Get",
    url: CATEGORY_MANAGEMENT_URL + `/${id}`,
    headers: HEADERS_CONFIG,
  })
    .then((res) => {
      return res.data;
    })
    .then(callBack);
};

let renderEdit = (cate) => {
  $("#edit-form input[id='id']").val(cate.id);
  $("#edit-form input[name='Title']").val(cate.title);
  $("#edit-form input[name='Description']").val(cate.description);
};

let renderDelete = (cate) => {
  $("#delete-form input[id='id']").val(cate.id);
  $("#delete-form input[name='Title']").val(cate.title);
};

$("#edit-form").submit(() => {
  let title = $("#edit-form input[name='Title']").val();
  let description = $("#edit-form input[name='Description']").val();
  let id = $("#edit-form input[id='id']").val();
  axios({
    method: "PUT",
    url: CATEGORY_MANAGEMENT_URL,
    data: {
      id: id,
      title: title,
      description: description,
    },
    headers: HEADERS_CONFIG,
  }).then(getDataList(1,""));
});

$("#delete-form").submit(() => {
  let id = $("#delete-form input[id='id']").val();
  axios({
    method: "DELETE",
    url: CATEGORY_MANAGEMENT_URL + `/${id}`,
    headers: HEADERS_CONFIG,
  }).then(getDataList(1, ""));
});

$("#create-form").submit(()=>{
  let title = $("#create-form input[name='Title']").val();
  let desc = $("#create-form input[name='Description']").val();
  axios({
    method: "POST",
    url: CATEGORY_MANAGEMENT_URL,
    data: {
      title: title,
      description: desc,
    },
    headers: HEADERS_CONFIG,
  });
})

$("#searchBtn").click(()=>{
  let search = $("input[id='searchString']").val();
  getDataList(1, search)
})