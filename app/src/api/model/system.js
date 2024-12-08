import config from '@/config';
import http from '@/utils/request';

export default {
    application: {
        detail: {
            url: `${config.API_URL}/app/get`,
            name: '获取应用',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/app/create`,
            name: '创建应用',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/app/update`,
            name: '更新应用',
            post: async function (data) {
                return await http.post(this.url, data);
            },
        },
        delete: {
            url: `${config.API_URL}/app/delete`,
            name: '删除应用',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
    businessUnit: {
		list: {
            url: `${config.API_URL}/businessUnit/getAllList`,
            name: '获取业务单元列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/businessUnit/get`,
            name: '获取业务单元',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/businessUnit/create`,
            name: '创建业务单元',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/businessUnit/update`,
            name: '更新业务单元',
            post: async function (data) {
                return await http.post(this.url, data);
            },
        },
        delete: {
            url: `${config.API_URL}/businessUnit/delete`,
            name: '删除业务单元',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
    page: {
		list: {
            url: `${config.API_URL}/page/getList`,
            name: '获取页面列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/page/get`,
            name: '获取页面',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/page/create`,
            name: '创建页面',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/page/update`,
            name: '更新页面',
            post: async function (data) {
                return await http.post(this.url, data);
            },
        },
        delete: {
            url: `${config.API_URL}/page/delete`,
            name: '删除页面',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
   
	agent: {
		list: {
            url: `${config.API_URL}/agent/getList`,
            name: '获取页面列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/agent/get`,
            name: '获取页面',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/agent/create`,
            name: '创建页面',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/agent/update`,
            name: '更新页面',
            post: async function (data) {
                return await http.post(this.url, data);
            },
        },
        delete: {
            url: `${config.API_URL}/agent/delete`,
            name: '删除页面',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
        execute: {
            url: `${config.API_URL}/agent/execute`,
            name: '执行',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
};
