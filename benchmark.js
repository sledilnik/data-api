import http from 'k6/http';
import { check } from 'k6';

const HOST = __ENV.BENCH_HOST

export default function () {
    let r = http.get(`${HOST}/api/stats`);
    check(r, {
        'status is 200': (r) => r.status === 200,
    });
}