import {HEADERS_CONFIG, REPORT_MANAGEMENT_URL} from './config.js'

let topRate = () =>{
    axios({
        method: "GET",
        url: REPORT_MANAGEMENT_URL + "/TopRatePost",
        headers: HEADERS_CONFIG
    }).then((res)=>{
        renderTopRate(res.data);
    })
}

let renderTopRate = (posts) =>{
    let startTable = `<table class="table table-striped">
    <thead>
        <tr>
            <th>Title</th>
            <th>Rate</th>
            <th>View</th>
        </tr>
    </thead>
    <tbody>`
    let endTable = `</tbody>
    </table>`
    const htmls = posts.map((post) =>{
        return `<tr>
        <th><a href="/Posts/Detail/${post.id}">${post.title}</a></th>
        <th>${post.rate}</th>
        <th>${post.views}</th>
        </tr>`
    })
    const table = startTable + htmls.join("") + endTable
    $("#topRate").append(table)
}

let topCate = () =>{
    axios({
        method: "GET",
        url: REPORT_MANAGEMENT_URL + "/TopCategories",
        headers: HEADERS_CONFIG
    }).then((res)=>{
        renderTopCate(res.data)
    })
}

let renderTopCate = (categories) => {
    let startTable = `<table class="table table-striped">
    <thead>
        <tr>
            <th>Title</th>
            <th>View</th>
        </tr>
    </thead>
    <tbody>`
    let endTable = `</tbody>
    </table>`
    const htmls = categories.map((category) =>{
        return `<tr>
        <th><a href="/Categories/Detail/${category.key.id}">${category.key.title}</a></th>
        <th>${category.value}</th>
        </tr>`
    })
    const table = startTable + htmls.join("") + endTable
    $("#topCategories").append(table)
}

let loadPage = () =>{
    topRate();
    topCate();
}

loadPage()