import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import authService from '@/services/auth-service';
import { ActivityActionType } from '@/api/clients';
import { useActivitySessionStore } from '@/stores/activity-session';

export function getRoutePathByActionType(type: ActivityActionType): string {
  switch (type) {
    case ActivityActionType.MONSTER_FIGHT:
        return '/fight';
      default:
        throw Error('Unknown type of activity action ' + type);
  }
}

export function getAllActivitiesRoutes(): string[] {
  return [
    '/fight',
  ];
}

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'barracks',
    component: () => import('@/pages/Barracks.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true 
    },
  },
  {
    path: '/create-hero',
    name: 'about',
    component: () => import('@/pages/CreateHero.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true 
    },
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/pages/Login.vue'),
    meta: {
      requiresAuth: false,
      requiresAnonimous: true,
    },
  },
  {
    path: '/registration',
    name: 'registration',
    component: () => import('@/pages/Registration.vue'),
    meta: {
      requiresAuth: false,
      requiresAnonimous: true,
    },
  },
  {
    path: '/arena',
    name: 'arena',
    component: () => import('@/pages/Arena.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true 
    },
  },
  {
    path: '/fight',
    name: 'fight',
    component: () => import('@/pages/Fight.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true 
    },
  },
  {
    path: '/statistic',
    name: 'statistic',
    component: () => import('@/pages/Statistic.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true 
    },
  },
  {
    path: '/hero-info/:id',
    name: 'hero-info',
    component: () => import('@/pages/HeroInfo.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true
    },
  },
  {
    path: '/shop',
    name: 'shop',
    component: () => import('@/pages/Shop.vue'),
    meta: {
      requiresAnonimous: false,
      requiresAuth: true,
    },
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

router.beforeEach(async (to, from) => {

  await authService.checkIsTokenAlive();

  if (to.meta.requiresAuth && !authService.isAuthenticated()) {
    return {
      path: '/login',
    };
  }
  if (to.meta.requiresAnonimous && authService.isAuthenticated()) {
    return {
      path: '/',
    };
  }

  if (authService.isAuthenticated()) {
    const activitySessionStore = useActivitySessionStore();

    await activitySessionStore.getCurrent();
    if (activitySessionStore.session) {
      const sessionRoute = getRoutePathByActionType(activitySessionStore.session.currentAction.type);
      if (to.path !== sessionRoute) {
        return {
          path: sessionRoute,
        };
      }
    } else if (getAllActivitiesRoutes().includes(to.path)) {
      return {
        path: '/',
      };
    }
  }
});

export default router;