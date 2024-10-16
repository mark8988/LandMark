<script>
import Multiselect from "@vueform/multiselect";
import "@vueform/multiselect/themes/default.css";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";

import Layout from "@/layouts/main.vue";
import PageHeader from "@/components/page-header";
import Swal from "sweetalert2";
import axios from 'axios';
import animationData from "@/components/widgets/msoeawqm.json";
import animationData1 from "@/components/widgets/gsqxdxog.json";
import Lottie from "@/components/widgets/lottie.vue";
//import simplebar from "simplebar-vue";
export default {
  data() {
    return {
      taskListModal: false,
      date3: null,
      rangeDateconfig: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        altInput: true,
        dateFormat: "d M, Y",
        mode: "range",
      },
      config: {
        wrap: true, // set wrap to true only when using 'input-group'
        altFormat: "M j, Y",
        altInput: true,
        dateFormat: "d M, Y",
      },
      timeConfig: {
        enableTime: false,
        dateFormat: "d M, Y",
      },
      filterdate: null,
      filterdate1: null,
      filtervalue: 'All',
      filtervalue1: 'All',
      filtersearchQuery1: null,
      date: null,
      allTask: [],
      searchQuery: null,
      page: 1,
      perPage: 8,
      pages: [],
      defaultOptions: {
        animationData: animationData
      },
      defaultOptions1: {
        animationData: animationData1
      },

      //
      submitted: false,

      dataEdit: false,
      deleteModal: false,
      event: {
        _id: "",
        taskId: "",
        task: "",
        creater: "",
        dueDate: "",
        priority: "",
        project: "",
        subItem: [],
        status: ""
      },
      //

    };
  },
  components: {
    Layout,
    PageHeader,
    lottie: Lottie,
    Multiselect,
    flatPickr
    //simplebar
  },
  computed: {
    displayedPosts() {
      return this.paginate(this.allTask);
    },
    resultQuery() {
      if (this.searchQuery) {
          console.log("displayedPosts: ", this.displayedPosts)
          return this.displayedPosts.filter((data) => {
          return (
              data.typename.includes(this.searchQuery) ||
              data.level.includes(this.searchQuery)
          );
        });
      } else if (this.filterdate !== null) {
        if (this.filterdate !== null) {
          var date1 = this.filterdate.split(" to ")[0];
          var date2 = this.filterdate.split(" to ")[1];
        }
        return this.displayedPosts.filter((data) => {
          if (
            new Date(data.dueDate.slice(0, 12)) >= new Date(date1) &&
            new Date(data.dueDate.slice(0, 12)) <= new Date(date2)
          ) {
            return data;
          } else {
            return null;
          }
        });
      } else if (this.filtervalue !== null) {
        return this.displayedPosts.filter((data) => {
          if (data.status === this.filtervalue || this.filtervalue == 'All') {
            return data;
          } else {
            return null;
          }
        });
      } else {
        return this.displayedPosts;
      }
    },
  },
  watch: {
    allTask() {
      this.setPages();
    },
  },
  created() {
    this.setPages();
  },
  filters: {
    trimWords(value) {
      return value.split(" ").splice(0, 20).join(" ") + "...";
    },
  },
  beforeMount() {
    axios.get('https://api-node.themesbrand.website/apps/task').then((data) => {
      this.allTask = [];
      const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec"
        ];
        console.log("data: ", data)
        //let result = data.data.data;
      data.data.data.forEach(row => {
        var dd = new Date(row.dueDate);
        row.dueDate = dd.getDate() + " " + monthNames[dd.getMonth()] + ", " + dd.getFullYear();
        row.subItem.forEach(imag => {
          imag.image_src = 'https://api-node.themesbrand.website/images/users/' + imag.img;
        });

        // row.image_src = `@/assets/images/products/img-8.png`;
          // row.image_src = 'https://api-node.themesbrand.website/fileupload/users_bucket/' + row.img;
          this.allTask = [
              {
                  "_id": "6260f5baae650a7cd1595918",
                  "typename": "公司",
                  "level": "1",
                  "taskId": "#VLZ460",
                  "project": "Qexal - Landing Page",
                  "task": "Brand logo design",
                  "creater": "David Nichols",
                  "dueDate": "2021-12-28T18:30:00.000Z",
                  "status": "Pending",
                  "priority": "High",
                  "user_id": "629f15c770a470a230cc5d5a"
              },
              {
                  "_id": "6260f5baae650a7cd1595902",
                  "typename": "品牌",
                  "level": "323",
                  "taskId": "#VLZ454",
                  "project": "Skote - v2.3.0",
                  "task": "Apologize for shopping Error!",
                  "creater": "Nathan Cole",
                  "dueDate": "2021-10-22T18:30:00.000Z",
                  "status": "Completed",
                  "priority": "Medium",
                  "user_id": "629f15c770a470a230cc5d5a"
              },
              {
                  "_id": "6260f5baae650a7cd15958fb",
                  "typename": "門市",
                  "level": "2",
                  "taskId": "#VLZ632",
                  "project": "Velzon - v1.0.0",
                  "task": "Error message when placing an orders?",
                  "creater": "Robert McMahon",
                  "dueDate": "2022-01-24T18:30:00.000Z",
                  "status": "Inprogress",
                  "priority": "High",
                  "user_id": "629f15c770a470a230cc5d5a"
              }
          ];

        //this.allTask.push(row);
      });
    }).catch((er) => {
      console.log(er);
    });

  },

  methods: {
    // 
    handleSubmit() {
          if (this.dataEdit) {
        this.submitted = true;
              if (this.submitted && (this.event.typename && this.event.level)) {
                  this.taskListModal = false;
          //
                  this.allTask = this.allTask.map(item => item._id.toString() == this.event._id.toString() ? { ...item, ...this.event } : item);
          //axios.patch(`https://api-node.themesbrand.website/apps/task/${this.event._id}`, this.event)
          //  .then((response) => {
          //    const data = response.data.data;
          //    this.allTask = this.allTask.map(item => item._id.toString() == data._id.toString() ? { ...item, ...data } : item);
          //  }).catch((er) => {
          //    console.log(er);
          //  });
        }
      } else {
              this.submitted = true;
              if (this.submitted && (this.event.typename && this.event.level)) {
          const data = {
            _id: (Math.floor(Math.random() * 100 + 20) - 20),
            taskId: '#VLZ4' + (Math.floor(Math.random() * 100 + 20) - 20),
            ...this.event
          };
            this.taskListModal = false;
            this.allTask.unshift(data);
          //axios.post(`https://api-node.themesbrand.website/apps/task`, data)
          //  .then((response) => {
          //    this.allTask.unshift(response.data.data);
          //  }).catch((er) => {
          //    console.log(er);
          //  });
        }
      }
    },

    onSort(column) {
      this.direction = this.direction === 'asc' ? 'desc' : 'asc';
      const sortedArray = [...this.allTask];
      sortedArray.sort((a, b) => {
        const res = a[column] < b[column] ? -1 : a[column] > b[column] ? 1 : 0;
        return this.direction === 'asc' ? res : -res;
      });
      this.allTask = sortedArray;
    },

    editDetails(data) {
      this.dataEdit = true;
      this.taskListModal = true;
      this.event = { ...data };

      this.submitted = false;
    },

    toggleModal() {
      this.taskListModal = true;
      this.dataEdit = false;
      this.event = {};

      this.submitted = false;
    },

    deleteModalToggle(data) {
      this.deleteModal = true;
      this.event._id = data._id;
    },

    deleteData() {
      if (this.event._id) {
        axios.delete(`https://api-node.themesbrand.website/apps/task/${this.event._id}`)
          .then((response) => {
            if (response.data.status === 'success') {
              this.allTask = this.allTask.filter((item) => item._id != this.event._id);
            }
          }).catch((er) => {
            console.log(er);
          });

        this.deleteModal = false;
      }
    },
    //

    SearchData() {
      this.filterdate = this.filterdate1;
      this.filtervalue = this.filtervalue1;
    },

    deleteMultiple() {
      let ids_array = [];
      var items = document.getElementsByName("chk_child");
      items.forEach(function (ele) {
        if (ele.checked == true) {
          var trNode = ele.parentNode.parentNode.parentNode;
          var id = trNode.querySelector(".id a").innerHTML;
          ids_array.push(id);
        }
      });
      if (typeof ids_array !== "undefined" && ids_array.length > 0) {
        if (confirm("Are you sure you want to delete this?")) {
          var cusList = this.allTask;
          ids_array.forEach(function (id) {
            cusList = cusList.filter(function (allTask) {
              return allTask.taskId != id;
            });
          });
          this.allTask = cusList;
          document.getElementById("checkAll").checked = false;
          var itemss = document.getElementsByName("chk_child");
          itemss.forEach(function (ele) {
            if (ele.checked == true) {
              ele.checked = false;
              ele.closest("tr").classList.remove("table-active");
            }
          });
        } else {
          return false;
        }
      } else {
        Swal.fire({
          title: "Please select at least one checkbox",
          confirmButtonClass: "btn btn-info",
          buttonsStyling: false,
          showCloseButton: true,
        });
      }
    },

    setPages() {
      let numberOfPages = Math.ceil(this.allTask.length / this.perPage);
      this.pages = [];
      for (let index = 1; index <= numberOfPages; index++) {
        this.pages.push(index);
      }
    },
    paginate(allTask) {
      let page = this.page;
      let perPage = this.perPage;
      let from = page * perPage - perPage;
      let to = page * perPage;
      return allTask.slice(from, to);
    },
  },
  mounted() {
    var checkAll = document.getElementById("checkAll");
    if (checkAll) {
      checkAll.onclick = function () {
        var checkboxes = document.querySelectorAll(
          '.form-check-all input[type="checkbox"]'
        );

        if (checkAll.checked == true) {
          checkboxes.forEach(function (checkbox) {
            checkbox.checked = true;
            checkbox.closest("tr").classList.add("table-active");
            document.getElementById('remove-actions').style.display = 'block';
          });
        } else {
          checkboxes.forEach(function (checkbox) {
            checkbox.checked = false;
            checkbox.closest("tr").classList.remove("table-active");
            document.getElementById('remove-actions').style.display = 'none';
          });
        }
      };
    }

    var checkboxes = document.querySelectorAll('#tasksTable .form-check-input');
    Array.from(checkboxes).forEach(function (element) {
      element.addEventListener('change', function (event) {
        var checkedCount = document.querySelectorAll('#tasksTable .form-check-input:checked').length;

        if (event.target.closest("tr").classList.contains("table-active")) {
          (checkedCount > 0) ? document.getElementById("remove-actions").style.display = 'block' : document.getElementById("remove-actions").style.display = 'none';
        } else {
          (checkedCount > 0) ? document.getElementById("remove-actions").style.display = 'block' : document.getElementById("remove-actions").style.display = 'none';
        }
      });
    });
  },
};
</script>
<template>
    <Layout>
        <PageHeader title="組織類型管理" pageTitle="管理" />
        <BRow>
            <BCol lg="12">
                <BCard no-body id="tasksList">
                    <BCardHeader class="border-0">
                        <div class="d-flex align-items-center">
                            <h5 class="card-title mb-0 flex-grow-1">組織類型</h5>
                            <div class="flex-shrink-0">
                                <div class="d-flex flex-wrap gap-2">
                                    <BButton variant="soft-danger" class="me-1" id="remove-actions" @click="deleteMultiple">
                                        <i class="ri-delete-bin-2-line"></i>
                                    </BButton>
                                    <BButton variant="danger" class="add-btn" @click="toggleModal">
                                        <i class="ri-add-line align-bottom me-1"></i> 新增組織類型
                                    </BButton>
                                </div>
                            </div>
                        </div>
                    </BCardHeader>
                    <BCardBody class="border border-dashed border-end-0 border-start-0">
                        <b-form>
                            <BRow class="g-3">
                                <BCol xxl="5" sm="12">
                                    <div class="search-box">
                                        <input type="text" class="form-control search bg-light border-light"
                                               placeholder="關鍵字搜尋..." v-model="searchQuery" />
                                        <i class="ri-search-line search-icon"></i>
                                    </div>
                                </BCol>

                                <BCol xxl="3" sm="4" v-if="false">
                                    <flat-pickr v-model="filterdate1" placeholder="Select date" :config="rangeDateconfig"
                                                class="form-control bg-light border-light"></flat-pickr>
                                </BCol>

                                <BCol xxl="3" sm="4" v-if="false">
                                    <div class="input-light">
                                        <Multiselect v-model="filtervalue1" :close-on-select="true" :searchable="true" :create-option="true"
                                                     :options="[
                        { value: 'All', label: 'All' },
                        { value: 'New', label: 'New' },
                        { value: 'Pending', label: 'Pending' },
                        { value: 'Inprogress', label: 'Inprogress' },
                        { value: 'Completed', label: 'Completed' },
                      ]" />
                                    </div>
                                </BCol>
                                <BCol xxl="1" sm="4">
                                    <BButton type="button" variant="primary" class="w-100" @click="SearchData">
                                        <i class="ri-equalizer-fill me-1 align-bottom"></i>
                                        篩選
                                    </BButton>
                                </BCol>
                            </BRow>
                        </b-form>
                    </BCardBody>
                    <BCardBody>
                        <div class="table-responsive table-card mb-4">
                            <table class="table align-middle table-nowrap mb-0" id="tasksTable">
                                <thead class="table-light text-muted">
                                    <tr>
                                        <!--<th scope="col" style="width: 40px">
                                          <div class="form-check">
                                            <input class="form-check-input" type="checkbox" id="checkAll" value="option" />
                                          </div>
                                        </th>-->
                                        <!--<th class="sort" data-sort="id" @click="onSort('taskId')">ID</th>-->
                                        <th class="sort" data-sort="org_name" @click="onSort('project')">組織類型</th>
                                        <th class="sort" data-sort="tasks_name" @click="onSort('task')">組織階層</th>
                                        <th data-sort="client_name">操作</th>
                                    </tr>
                                </thead>
                                <tbody class="list form-check-all">
                                    <tr v-for="(task, index) of resultQuery" :key="index">
                                        <!--<th scope="row">
      <div class="form-check">
        <input class="form-check-input" type="checkbox" name="chk_child" value="option1" />
      </div>
    </th>-->
                                        <!--<td class="id">
      <router-link to="/apps/tasks-details" class="fw-medium link-primary">{{ task.taskId }}
      </router-link>
    </td>-->
                                        <td class="project_name">
                                            <router-link to="/apps/projects-overview" class="fw-medium link-primary">
                                                {{ task.typename }}
                                            </router-link>
                                        </td>
                                        <td class="project_name">
                                            <router-link to="/apps/projects-overview" class="fw-medium link-primary">
                                                {{ task.level }}
                                            </router-link>
                                        </td>
                                        <td>
                                            <div class="hstack gap-3 flex-wrap">
                                                <a href="javascript:void(0);" class="link-success fs-15" @click="editDetails(task)">
                                                    <i class="ri-edit-2-line"></i>
                                                </a>
                                                <a href="javascript:void(0);" class="link-danger fs-15" @click="deleteModalToggle(task)">
                                                    <i class="ri-delete-bin-line"></i>
                                                </a>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="noresult" v-if="resultQuery.length < 1">
                                <div class="text-center">
                                    <lottie colors="primary:#121331,secondary:#08a88a" :options="defaultOptions" :height="75" :width="75" />
                                    <h5 class="mt-2">Sorry! No Result Found</h5>
                                    <p class="text-muted mb-0">
                                        We've searched more than 200k+ tasks We did not find any
                                        tasks for you search.
                                    </p>
                                </div>
                            </div>
                        </div>

                        <div class="d-flex justify-content-end" v-if="resultQuery.length >= 1">
                            <div class="pagination-wrap hstack gap-2">
                                <BLink class="page-item pagination-prev" href="#" :disabled="page <= 1" @click="page--">
                                    上一頁
                                </BLink>
                                <ul class="pagination listjs-pagination mb-0">
                                    <li :class="{ active: pageNumber == page, disabled: pageNumber == '...', }"
                                        v-for="(pageNumber, index) in pages" :key="index" @click="page = pageNumber">
                                        <BLink class="page" href="#" data-i="1" data-page="8">{{ pageNumber }}</BLink>
                                    </li>
                                </ul>
                                <BLink class="page-item pagination-next" href="#" :disabled="page >= pages.length" @click="page++">
                                    下一頁
                                </BLink>
                            </div>
                        </div>
                    </BCardBody>
                </BCard>
            </BCol>
        </BRow>

        <!-- task list modal -->
        <BModal v-model="taskListModal" id="showmodal" modal-class="zoomIn" hide-footer
                header-class="p-3 bg-info-subtle taskModal" class="v-modal-custom" centered size="lg"
                :title="dataEdit ? '編輯' : '新增'">
            <b-form id="addform" class="tablelist-form" autocomplete="off">
                <BRow class="g-3">
                    <input type="hidden" id="id" name="">
                    <BCol lg="6">
                        <label for="projectName-field" class="form-label">組織類型名稱</label>
                        <input type="text" id="projectName" class="form-control" placeholder="組織名稱" v-model="event.typename"
                               :class="{ 'is-invalid': submitted && !event.typename }" />
                        <div class="invalid-feedback">名稱未填</div>
                    </BCol>
                    <BCol lg="6">
                        <div>
                            <label for="tasksTitle-field" class="form-label">組織層級</label>
                            <input type="text" id="tasksTitle" class="form-control" placeholder="組織層級" v-model="event.level"
                                   :class="{ 'is-invalid': submitted && !event.level }" />
                            <div class="invalid-feedback">層級未填</div>
                        </div>
                    </BCol>
                    <BCol lg="12">
                        <label for="createName-field" class="form-label">備註</label>
                        <textarea id="createName" class="form-control" placeholder="備註" v-model="event.memo" rows="4"></textarea>
                    </BCol>
                </BRow>

                <div class="hstack gap-2 justify-content-end mt-3">
                    <BButton type="button" variant="light" @click="taskListModal = false" id="closemodal"> 關閉 </BButton>
                    <BButton type="submit" variant="success" id="add-btn" @click="handleSubmit">
                        {{ dataEdit ? '更新' : '新增' }}
                    </BButton>
                </div>
            </b-form>
        </BModal>

        <!-- delete modal -->
        <BModal v-model="deleteModal" modal-class="zoomIn" hide-footer no-close-on-backdrop centered>
            <div class="mt-2 text-center">
                <lottie class="avatar-xl" colors="primary:#f7b84b,secondary:#f06548" :options="defaultOptions1" :height="75"
                        :width="75" />
                <div class="mt-4 pt-2 fs-15 mx-4 mx-sm-5">
                    <h4>刪除確認</h4>
                    <p class="text-muted mx-4 mb-0">是否確定刪除此組織類型?</p>
                </div>
            </div>
            <div class="d-flex gap-2 justify-content-center mt-4 mb-2">
                <BButton variant="light" size="w-sm" @click="deleteModal = false">關閉</BButton>
                <BButton variant="danger" size="w-sm" id="delete-record" @click="deleteData">確定刪除</BButton>
            </div>
        </BModal>
    </Layout>
</template>
