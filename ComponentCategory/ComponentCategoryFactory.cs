using UnityEngine;
using UnityEditor;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Corelib.Utils
{
    using Category = ComponentCategoryType;

    [Serializable]
    public enum ComponentCategoryType
    {
        All,
        Rendering,
        Lighting,
        Input,
        Physics,
        Audio,
        Particle,
        Camera,
        UI,
        Animation,
        User,
        None,
    }

    public static class ComponentCategoryFactory
    {
        private static readonly List<Type> categoryRendering = new(){
            typeof(MeshRenderer),
        };
        private static readonly List<Type> categoryLighting = new(){
            typeof(Light),
        };
        private static readonly List<Type> categoryInput = new()
        {

        };
        private static readonly List<Type> categoryPhysics = new(){
            typeof(Rigidbody),
            typeof(Rigidbody2D),
        };
        private static readonly List<Type> categoryAudio = new(){
            typeof(AudioSource),
            typeof(AudioListener),
        };
        private static readonly List<Type> categoryParticle = new(){
            typeof(ParticleSystem),
        };
        private static readonly List<Type> categoryCamera = new(){
            typeof(Camera),
        };
        private static readonly List<Type> categoryUI = new(){
            typeof(Canvas),
            typeof(Image),
        };
        private static readonly List<Type> categoryAnimation = new(){
            typeof(Animator),
        };

        private static readonly Dictionary<Category, List<Type>> categoryComponent = new(){
            {Category.Rendering, categoryRendering},
            {Category.Lighting, categoryLighting},
            {Category.Input, categoryInput},
            {Category.Physics, categoryPhysics},
            {Category.Audio, categoryAudio},
            {Category.Particle, categoryParticle},
            {Category.Camera, categoryCamera},
            {Category.UI, categoryUI},
            {Category.Animation, categoryAnimation},
        };

        public static Category GetCategory(Component component)
        {
            IEnumerable<Category> categories =
                Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Where((category) => !new List<Category>(){
                    Category.All,
                    Category.User,
                    Category.None,
                }.Contains(category));
            foreach (var category in categories)
            {
                if (categoryComponent[category].Contains(component.GetType()))
                {
                    return category;
                }
            }
            return Category.None;
        }

        public static bool InCategory(Category category, Component component)
        {
            switch (category)
            {
                case Category.All:
                    {
                        return true;
                    }
                case Category.User:
                    {
                        string namespaceName = component.GetType().Namespace ?? "No";
                        return !namespaceName.Equals("UnityEngine");
                    }
                default:
                    {
                        return GetCategory(component) == category;
                    }
            }
        }
    }
}