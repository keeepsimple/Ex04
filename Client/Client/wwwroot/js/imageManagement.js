import {
  IMAGECATEGORY_MANAGEMENT_URL,
  IMAGE_MANAGEMENT_URL,
  HEADERS_MULTIPART_CONFIG,
  HEADERS_CONFIG,
  SERVER_URL,
} from "./config.js";

const preview = document.getElementById("preview");

$("#create-form").submit(() => {
  let name = $("input[name='Name']").val();
  let file = document.querySelector("input[type=file]").files[0];
  let imageCateIds = $("#CreateImageCateIds").select2("val");
  let height = preview.naturalHeight;
  let width = preview.naturalWidth;
  axios({
    method: "Post",
    url: IMAGE_MANAGEMENT_URL,
    data: {
      imageUrl: "123",
      name: name,
      size: file.size,
      height: height,
      width: width,
      uploadImage: file,
      imageCateIds: imageCateIds,
    },
    headers: HEADERS_MULTIPART_CONFIG,
  });
});

$("#createBtn").click(() => {
  getCategories(renderImageCateCreate);
});

let getCategories = (callback) => {
  axios({
    method: "GET",
    url: IMAGECATEGORY_MANAGEMENT_URL,
    headers: HEADERS_CONFIG,
  })
    .then((res) => {
      return res.data;
    })
    .then(callback);
};

let filterImage = (imageCates) => {
  const htmls = imageCates.map((imageCate) => {
    return `<option value="${imageCate.id}" href="">${imageCate.name}</option>`;
  });

  const startDropdown = `<div class="mb-3 col-md-4"><select id="imageCate" class="form-select" onchange="module.changePage(event,1)"><option value="" selected>Select an Image Category</option>`;
  const endDropdown = `</select></div><div class="mb-3 col-md-4">
  <select id="sort" class="form-select" onchange="module.changePage(event,1)">
  <option value="" selected>Created At Descending</option>
  <option value="createdAt_asc">Created At Ascending</option>
  <option value="Name">Name Descending</option>
  <option value="name_asc">Name Ascending</option>
  </select>
</div>`;

  $("#filterImage").append(startDropdown + htmls.join("") + endDropdown);
};

let renderImageCateCreate = (imageCates) => {
  const htmls = imageCates.map((imageCate) => {
    return `<option value="${imageCate.id}">${imageCate.name}</option>`;
  });

  $("select#CreateImageCateIds").html(htmls.join(""));
};

let renderImages = (images) => {
  const htmls = images.map((image) => {
    return `<div id="imageItem" class="card col-md-4 mx-2 mb-3" style="width: 18rem;">
        <img src="${SERVER_URL}/images/${
      image.imageUrl
    }" class="card-img-top" alt="${image.name}">
        <div class="card-body">
            <h5 class="card-title">${image.name}</h5>
            <p class="card-text">Size: ${image.size} KB</p>
            <p class="card-text">Width x Height: ${image.width} x ${
      image.height
    }</p>
            <p class="card-text">Upload at: ${convertDate(image.createdAt)}</p>
        </div>
        <button id="deleteBtn" class="btn btn-outline-danger btn-icon-split" data-bs-toggle="modal" data-bs-target="#deleteModal">Delete
            </button>
            <input type="hidden" name="id" id="id" value="${image.id}" />
    </div>`;
  });

  $("#imageBlock").html(htmls.join(""));

  $("#imageItem > #deleteBtn").click((e) => {
    let id = $(e.target).parent().find("#id").val();
    getImage(id);
  });
};

function convertDate(date) {
  date = new Date(date);
  return date.toDateString();
}

const getDataList = (item, searchString, sortOrder, imageCate) => {
  let pageNum = item;
  let pageSize = 4;
  axios({
    method: "get",
    url:
      IMAGE_MANAGEMENT_URL +
      `/GetPagination/${pageNum}?sortOrder=${sortOrder}&imageCategory=${imageCate}&searchString=${searchString}&pageSize=${pageSize}`,
    headers: HEADERS_CONFIG,
  }).then((response) => {
    renderImages(response.data.list);
    renderPaging(response.data.count, item, pageSize);
  });
};

const load = () => {
  getDataList(1, "", "", "");
  getCategories(filterImage);
};

load();

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
  const imageCate = $("#imageCate").find(":selected").val()
  const sortOrder = $("#sort").find(":selected").val()
  getDataList(item, search, sortOrder, imageCate);
};

let getImage = (id) => {
  axios({
    method: "Get",
    url: IMAGE_MANAGEMENT_URL + `/${id}`,
    headers: HEADERS_CONFIG,
  }).then((res) => {
    document.getElementById(
      "imageDelete"
    ).src = `${SERVER_URL}/images/${res.data.imageUrl}`;
    $("input[id='id']").val(res.data.id);
  });
};

$("#delete-form").submit(() => {
  const id = $("input[id='id']").val();
  axios({
    method: "Delete",
    url: IMAGE_MANAGEMENT_URL + `/${id}`,
    headers: HEADERS_CONFIG,
  }).then(getDataList(1));
});

$("#searchBtn").click(() => {
  let search = $("input[id='searchString']").val();
  getDataList(1, search, sortOrderFilter, imageCateFilter);
});
